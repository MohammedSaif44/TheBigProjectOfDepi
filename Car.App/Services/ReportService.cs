using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarRental.App.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repo;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public ReportService(IReportRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;

            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        private async Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> getData)
        {
            // 1) check redis
            var cached = await _cache.GetStringAsync(key);
            if (cached != null)
                return JsonSerializer.Deserialize<T>(cached)!;

            // 2) get from repo
            var data = await getData();

            // 3) save to redis
            var json = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key, json, _cacheOptions);

            return data;
        }

        public Task<GeneralStatsDto> GetGeneralStatsAsync()
            => GetOrSetCacheAsync("general_stats", () => _repo.GetGeneralStatsAsync());

        public Task<IEnumerable<TopCarDto>> GetTopCarsAsync(int take)
            => GetOrSetCacheAsync($"top_cars_{take}", () => _repo.GetTopCarsAsync(take));

        public Task<IEnumerable<DailyRevenueDto>> GetDailyRevenueAsync(int days)
            => GetOrSetCacheAsync($"daily_revenue_{days}", () => _repo.GetDailyRevenueAsync(days));

        public Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months)
            => GetOrSetCacheAsync($"monthly_revenue_{months}", () => _repo.GetMonthlyRevenueAsync(months));

        public Task<IEnumerable<ReservationStatusDto>> GetReservationsByStatusAsync()
            => GetOrSetCacheAsync("reservation_status", () => _repo.GetReservationsByStatusAsync());

        public Task<IEnumerable<NewUsersPerMonthDto>> GetNewUsersPerMonthAsync(int months)
            => GetOrSetCacheAsync($"new_users_{months}", () => _repo.GetNewUsersPerMonthAsync(months));

        public Task<IEnumerable<PaymentDto>> GetPaymentsAsync(DateTime? from, DateTime? to)
            => GetOrSetCacheAsync($"payments_{from}_{to}", () => _repo.GetPaymentsAsync(from, to));
    }
}
