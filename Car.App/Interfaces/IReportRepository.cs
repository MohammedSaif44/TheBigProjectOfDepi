using CarRental.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface IReportRepository
    {
        Task<GeneralStatsDto> GetGeneralStatsAsync();
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalCarsAsync();
        Task<int> GetTotalReservationsAsync();
        Task<int> GetTotalPaymentsAsync();
        Task<decimal> GetTotalRevenueAsync();

        Task<IEnumerable<TopCarDto>> GetTopCarsAsync(int take);
        Task<IEnumerable<DailyRevenueDto>> GetDailyRevenueAsync(int days);
        Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int months);
        Task<IEnumerable<ReservationStatusDto>> GetReservationsByStatusAsync();
        Task<IEnumerable<NewUsersPerMonthDto>> GetNewUsersPerMonthAsync(int months);
        Task<IEnumerable<PaymentDto>> GetPaymentsAsync(DateTime? from, DateTime? to);
    }

}
