using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface INotificationService
    {
        Task SendInAppAsync(string userId, string title, string message, NotificationType type);

        Task SendEmailAsync(string userEmail, NotificationType type, Dictionary<string, string>? data = null);

        Task NotifyAsync(
            string userId,
            string userEmail,
            NotificationType type,
            string title,
            string message,
            Dictionary<string, string>? templateData = null);
    }

}
