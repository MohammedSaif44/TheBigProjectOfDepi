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
    public class NotificationPreferenceRepository : INotificationPreferenceRepository
    {
        private readonly ApplicationDbContext _ctx;

        public NotificationPreferenceRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<NotificationPreference?> GetByUserIdAsync(string userId)
        {
            return await _ctx.NotificationPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddOrUpdateAsync(NotificationPreference pref)
        {
            var existing = await GetByUserIdAsync(pref.UserId);

            if (existing == null)
                await _ctx.NotificationPreferences.AddAsync(pref);
            else
                _ctx.NotificationPreferences.Update(pref);

            await _ctx.SaveChangesAsync();
        }
    }
}
