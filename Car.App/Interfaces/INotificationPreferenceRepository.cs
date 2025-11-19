using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface INotificationPreferenceRepository
    {
        Task<NotificationPreference?> GetByUserIdAsync(string userId);
        Task AddOrUpdateAsync(NotificationPreference pref);
    }

}
