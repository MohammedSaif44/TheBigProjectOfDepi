using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CarRental.App.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IConfiguration _config;

        public PaymentService(IPaymentRepository paymentRepo, IReservationRepository reservationRepo, IConfiguration config)
        {
            _paymentRepo = paymentRepo;
            _reservationRepo = reservationRepo;
            _config = config;

            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<PaymentResultDto> CreateCheckoutSessionAsync(CreatePaymentDto dto, string userEmail)
        {
            var reservation = await _reservationRepo.GetByIdAsync(dto.ReservationId);

            if (reservation == null)
                throw new Exception("Reservation not found");

            if (reservation.TotalPrice <= 0)
                throw new Exception("Invalid reservation price");

            long amount = (long)(reservation.TotalPrice * 100);

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                CustomerEmail = userEmail,
                SuccessUrl = dto.SuccessUrl ?? "https://localhost:44385/api/payment/success",
                CancelUrl = dto.CancelUrl ?? "https://localhost:44385/api/payment/cancel",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = amount,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Car Reservation Payment",
                            Description = $"Reservation #{reservation.Id}"
                        }
                    }
                }
            },
                Metadata = new Dictionary<string, string>
                {
                { "reservationId", reservation.Id.ToString() }
            }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            // حفظ عملية الدفع
            var payment = new Payment
            {
                ReservationId = reservation.Id,
                Amount = reservation.TotalPrice,
                Status = "Pending",
                Method = "Stripe",
                SessionId = session.Id
            };

            await _paymentRepo.AddAsync(payment);

            return new PaymentResultDto
            {
                SessionId = session.Id,
                CheckoutUrl = session.Url
            };
        }

        // Webhook handler
        public async Task HandleCheckoutCompletedAsync(Session session)
        {
            if (session == null || string.IsNullOrEmpty(session.Id))
                return;

            var payment = await _paymentRepo.GetBySessionIdAsync(session.Id);
            if (payment == null)
                return;

            payment.Status = "Paid";
            payment.PaymentDate = DateTime.UtcNow;

            await _paymentRepo.UpdateAsync(payment);

            // تحديث حالة الحجز
            var reservation = await _reservationRepo.GetByIdAsync(payment.ReservationId);
            if (reservation != null)
            {
                reservation.Status = "Confirmed";
                await _reservationRepo.UpdateAsync(reservation);
            }
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var list = await _paymentRepo.GetAllAsync();
            return list.Select(p => new PaymentDto
            {
                Id = p.Id,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                Status = p.Status,
                Method = p.Method,
                SessionId = p.SessionId,
                PaymentDate = p.PaymentDate
            });
        }
    }
}
