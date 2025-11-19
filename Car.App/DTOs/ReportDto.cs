using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.DTOs
{
    // GeneralStatsDto
    public class GeneralStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalCars { get; set; }
        public int TotalReservations { get; set; }
        public int TotalPayments { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // KeyValueDto (generic)
    public class KeyValueDto
    {
        public string Key { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    // TopCarDto
    public class TopCarDto
    {
        public int CarId { get; set; }
        public string Model { get; set; } = string.Empty;
        public int RentedCount { get; set; }
    }

    // DailyRevenueDto
    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    // MonthlyRevenueDto
    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
    }

    // ReservationStatusDto
    public class ReservationStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    // NewUsersPerMonthDto
    public class NewUsersPerMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }

}
