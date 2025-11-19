using CarRental.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]   // التقارير للأدمن فقط
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        // ================== Dashboard (General Stats) ==================
        [HttpGet("general-stats")]
        public async Task<IActionResult> GetGeneralStats()
        {
            return Ok(await _service.GetGeneralStatsAsync());
        }

        // ================== Top Rented Cars ==================
        [HttpGet("top-cars")]
        public async Task<IActionResult> GetTopCars(int take = 5)
        {
            return Ok(await _service.GetTopCarsAsync(take));
        }

        // ================== Revenue (Daily) ==================
        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenue(int days = 7)
        {
            return Ok(await _service.GetDailyRevenueAsync(days));
        }

        // ================== Revenue (Monthly) ==================
        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue(int months = 6)
        {
            return Ok(await _service.GetMonthlyRevenueAsync(months));
        }

        // ================== Reservations by Status ==================
        [HttpGet("reservation-status")]
        public async Task<IActionResult> GetReservationStatus()
        {
            return Ok(await _service.GetReservationsByStatusAsync());
        }

        // ================== New Users Per Month ==================
        [HttpGet("new-users")]
        public async Task<IActionResult> GetNewUsers(int months = 6)
        {
            return Ok(await _service.GetNewUsersPerMonthAsync(months));
        }


        // ================== Payments Report ==================
        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments(DateTime? from, DateTime? to)
        {
            return Ok(await _service.GetPaymentsAsync(from, to));
        }
    }
}
