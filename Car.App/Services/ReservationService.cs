using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly ICarRepository _carRepo;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationService(IReservationRepository reservationRepo, ICarRepository carRepo, INotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _reservationRepo = reservationRepo;
            _carRepo = carRepo;
            _notificationService = notificationService;
            _userManager = userManager;
        }


        public async Task<int> AddAsync(string userId, CreateReservationDto dto)
        {
            var car = await _carRepo.GetByIdAsync(dto.CarId);
            if (car == null || car.Status != "Available")
                throw new Exception("Car is not available.");

            if (dto.EndDate <= dto.StartDate)
                throw new Exception("Invalid reservation dates.");

            var days = (dto.EndDate - dto.StartDate).Days;
            var total = car.PricePerDay * days;

            var reservation = new Reservation
            {
                UserId = userId,
                CarId = dto.CarId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = total,
                Status = "Pending"
            };

            await _reservationRepo.AddAsync(reservation);

            var user = await _userManager.FindByIdAsync(reservation.UserId);

            await _notificationService.NotifyAsync(
                reservation.UserId,
                user?.Email ?? "",
                NotificationType.ReservationCreated,
                "Reservation Created",
                $"Your reservation #{reservation.Id} has been created.",
                new Dictionary<string, string>
                {
            { "ReservationId", reservation.Id.ToString() },
            { "Status", reservation.Status }
                }
            );

            return reservation.Id;  // ← أهم حاجة
        }



        public async Task<IEnumerable<ReservationDto>> GetByUserAsync(string userId)
        {
            var list = await _reservationRepo.GetByUserIdAsync(userId);

            return list.Select(r => new ReservationDto
            {
                Id = r.Id,

                UserFullName = r.User.FullName,
                UserEmail = r.User.Email,

                CarMake = r.Car.Make,
                CarModel = r.Car.Model,
               

                TotalPrice = r.TotalPrice,
                Status = r.Status,
                StartDate = r.StartDate,
                EndDate = r.EndDate
            });
        }

       
        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var list = await _reservationRepo.GetAllAsync();

            return list.Select(r => new ReservationDto
            {
                Id = r.Id,

                UserFullName = r.User.FullName,
                UserEmail = r.User.Email,

                CarMake = r.Car.Make,
                CarModel = r.Car.Model,
               

                TotalPrice = r.TotalPrice,
                Status = r.Status,
                StartDate = r.StartDate,
                EndDate = r.EndDate
            });
        }


        public async Task<bool> UpdateAsync(UpdateReservationDto dto, string userId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(dto.Id);
            if (reservation == null)
                return false;

            // Get caller user
            var currentUser = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(currentUser);
            bool isAdmin = roles.Contains("Admin");

            // Allow: reservation owner OR admin
            if (reservation.UserId != userId && !isAdmin)
                throw new Exception("Not allowed.");

            // Update reservation
            var days = (dto.EndDate - dto.StartDate).Days;
            reservation.StartDate = dto.StartDate;
            reservation.EndDate = dto.EndDate;
            reservation.Status = dto.Status;
            reservation.TotalPrice = reservation.Car.PricePerDay * days;

            await _reservationRepo.UpdateAsync(reservation);

            // Send notification to the reservation owner
            var reservationOwner = await _userManager.FindByIdAsync(reservation.UserId);

            await _notificationService.NotifyAsync(
                reservation.UserId,
                reservationOwner?.Email ?? "",
                NotificationType.ReservationUpdated,
                "Reservation Updated",
                $"Your reservation #{reservation.Id} has been updated.",
                new Dictionary<string, string>
                {
            { "ReservationId", reservation.Id.ToString() },
            { "Status", reservation.Status }
                }
            );

            return true;
        }



        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var res = await _reservationRepo.GetByIdAsync(id);
            if (res == null) return false;

            // Get current user (the caller)
            var currentUser = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(currentUser);
            bool isAdmin = roles.Contains("Admin");

            // Allow admin OR reservation owner
            if (res.UserId != userId && !isAdmin)
                throw new Exception("Not allowed.");

            // Free the car again
            var car = await _carRepo.GetByIdAsync(res.CarId);
            if (car != null)
            {
                car.Status = "Available";
                await _carRepo.UpdateAsync(car);
            }

            // Delete reservation
            await _reservationRepo.DeleteAsync(id);

            // Notification should go to the RESERVATION OWNER
            var reservationOwner = await _userManager.FindByIdAsync(res.UserId);

            await _notificationService.NotifyAsync(
                res.UserId, // send to owner
                reservationOwner?.Email ?? "",
                NotificationType.ReservationCancelled,
                "Reservation Cancelled",
                $"Your reservation #{res.Id} has been cancelled.",
                new Dictionary<string, string>
                {
            { "ReservationId", res.Id.ToString() }
                }
            );

            return true;
        }



    }

}
