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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly INotificationPreferenceRepository _prefRepo;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateRepository _templateRepo;

        public NotificationService(
            INotificationRepository notificationRepo,
            INotificationPreferenceRepository prefRepo,
            IEmailSender emailSender,
            IEmailTemplateRepository templateRepo)
        {
            _notificationRepo = notificationRepo;
            _prefRepo = prefRepo;
            _emailSender = emailSender;
            _templateRepo = templateRepo;
        }

        // -------------------------
        // Send In-App Notification
        // -------------------------
        public async Task SendInAppAsync(string userId, string title, string message, NotificationType type)
        {
            var notif = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            };

            await _notificationRepo.AddAsync(notif);
        }

        // -------------------------
        // Send Email Notification
        // -------------------------
        public async Task SendEmailAsync(string userEmail, NotificationType type, Dictionary<string, string>? data = null)
        {
            string templateKey = type.ToString(); // ReservationCreated - PaymentFailed ...

            var template = await _templateRepo.GetByKeyAsync(templateKey);
            if (template == null)
                return;

            string subject = template.Subject;
            string body = template.Body;

            if (data != null)
            {
                foreach (var item in data)
                {
                    body = body.Replace($"{{{{{item.Key}}}}}", item.Value);
                }
            }

            await _emailSender.SendEmailAsync(userEmail, subject, body);
        }

        // -------------------------
        // Combined Notification Handler
        // -------------------------
        public async Task NotifyAsync(string userId, string userEmail, NotificationType type,
                                      string title, string message,
                                      Dictionary<string, string>? templateData = null)
        {
            var pref = await _prefRepo.GetByUserIdAsync(userId);

            if (pref == null)
            {
                pref = new NotificationPreference { UserId = userId };
                await _prefRepo.AddOrUpdateAsync(pref);
            }

            bool sendEmail = type switch
            {
                NotificationType.ReservationCreated => pref.EmailOnReservationCreated,
                NotificationType.ReservationUpdated => pref.EmailOnReservationUpdated,
                NotificationType.PaymentSuccess => pref.EmailOnPaymentSuccess,
                NotificationType.PaymentFailed => pref.EmailOnPaymentFailed,
                _ => true
            };

            await SendInAppAsync(userId, title, message, type);

            if (sendEmail)
                await SendEmailAsync(userEmail, type, templateData);
        }
    }
}
