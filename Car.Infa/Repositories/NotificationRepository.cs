using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infa.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _ctx;

        public NotificationRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(Notification notification)
        {
            await _ctx.Notifications.AddAsync(notification);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _ctx.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _ctx.Notifications.FindAsync(notificationId);
            if (notification == null) return;

            notification.IsRead = true;
            await _ctx.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var list = await _ctx.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var n in list)
                n.IsRead = true;

            await _ctx.SaveChangesAsync();
        }
    }
}
