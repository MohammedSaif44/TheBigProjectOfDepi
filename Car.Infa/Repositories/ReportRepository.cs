using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Infa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infa.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _ctx;

        public ReportRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        // ========== GENERAL STATS ==========
        public async Task<GeneralStatsDto> GetGeneralStatsAsync()
        {
            var result = new GeneralStatsDto
            {
                TotalUsers = await _ctx.Users.AsNoTracking().CountAsync(),
                TotalCars = await _ctx.Cars.AsNoTracking().CountAsync(),
                TotalReservations = await _ctx.Reservations.AsNoTracking().CountAsync(),
                TotalPayments = await _ctx.Payments.AsNoTracking().CountAsync(),
                TotalRevenue = await _ctx.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == "Paid")
                    .SumAsync(p => (decimal?)p.Amount) ?? 0
            };

            return result;
        }

        public Task<int> GetTotalUsersAsync()
            => _ctx.Users.AsNoTracking().CountAsync();

        public Task<int> GetTotalCarsAsync()
            => _ctx.Cars.AsNoTracking().CountAsync();

        public Task<int> GetTotalReservationsAsync()
            => _ctx.Reservations.AsNoTracking().CountAsync();

        public Task<int> GetTotalPaymentsAsync()
            => _ctx.Payments.AsNoTracking().CountAsync();

        public Task<decimal> GetTotalRevenueAsync()
        {
            return _ctx.Payments
                .AsNoTracking()
                .Where(p => p.Status == "Paid")
                .SumAsync(p => (decimal?)p.Amount)
                .ContinueWith(task => task.Result ?? 0m);
        }

        // ========== TOP CARS ==========
        public async Task<IEnumerable<TopCarDto>> GetTopCarsAsync(int take)
        {
            return await _ctx.Reservations
                .AsNoTracking()
                .Include(r => r.Car)
                .GroupBy(r => new { r.CarId, r.Car!.Model })
                .Select(g => new TopCarDto
                {
                    CarId = g.Key.CarId,
                    Model = g.Key.Model,
                    RentedCount = g.Count()
                })
                .OrderByDescending(x => x.RentedCount)
                .Take(take)
                .ToListAsync();
        }

        // ========== DAILY REVENUE ==========
        public async Task<IEnumerable<DailyRevenueDto>> GetDailyRevenueAsync(int days)
        {
            var from = DateTime.UtcNow.Date.AddDays(-days + 1);

            return await _ctx.Payments
                .AsNoTracking()
                .Where(p => p.Status == "Paid" && p.PaymentDate.Date >= from)
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        // ========== MONTHLY REVENUE ==========
        public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months)
        {
            var start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
                        .AddMonths(-months + 1);

            return await _ctx.Payments
                .AsNoTracking()
                .Where(p => p.Status == "Paid" && p.PaymentDate >= start)
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }

        // ========== RESERVATION STATUS ==========
        public async Task<IEnumerable<ReservationStatusDto>> GetReservationsByStatusAsync()
        {
            return await _ctx.Reservations
                .AsNoTracking()
                .GroupBy(r => r.Status)
                .Select(g => new ReservationStatusDto
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        // ========== NEW USERS ==========
        public async Task<IEnumerable<NewUsersPerMonthDto>> GetNewUsersPerMonthAsync(int months)
        {
            var start = DateTime.UtcNow.AddMonths(-months);

            return await _ctx.Users
                .AsNoTracking()
                .Where(u => u.CreatedAt >= start)
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .Select(g => new NewUsersPerMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }

        // ========== PAYMENTS ==========
        public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync(DateTime? from, DateTime? to)
        {
            var query = _ctx.Payments
                .AsNoTracking()
                .Include(p => p.Reservation)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(p => p.PaymentDate >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.PaymentDate <= to.Value);

            return await query
                .Select(p => new PaymentDto
                {
                    Id = p.Id,
                    ReservationId = p.ReservationId,
                    Amount = p.Amount,
                    Status = p.Status,
                    Method = p.Method,
                    SessionId = p.SessionId,
                    PaymentDate = p.PaymentDate
                })
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync();
        }
    }
}

