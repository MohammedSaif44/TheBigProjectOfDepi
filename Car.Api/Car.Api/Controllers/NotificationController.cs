using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly INotificationPreferenceRepository _prefRepo;

        public NotificationController(
            INotificationRepository notificationRepo,
            INotificationPreferenceRepository prefRepo)
        {
            _notificationRepo = notificationRepo;
            _prefRepo = prefRepo;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");


            var result = await _notificationRepo.GetUserNotificationsAsync(userId);
            return Ok(result);
        }

        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationRepo.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read" });
        }

        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");


            await _notificationRepo.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        [HttpGet("preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");


            var pref = await _prefRepo.GetByUserIdAsync(userId);
            return Ok(pref);
        }

        [HttpPut("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] NotificationPreferenceDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var pref = await _prefRepo.GetByUserIdAsync(userId);

            if (pref == null)
                pref = new NotificationPreference { UserId = userId };

            pref.EmailOnReservationCreated = dto.EmailOnReservationCreated;
            pref.EmailOnReservationUpdated = dto.EmailOnReservationUpdated;
            pref.EmailOnPaymentSuccess = dto.EmailOnPaymentSuccess;
            pref.EmailOnPaymentFailed = dto.EmailOnPaymentFailed;

            await _prefRepo.AddOrUpdateAsync(pref);

            return Ok(new { message = "Preferences updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var deleted = await _notificationRepo.DeleteNotificationAsync(id, userId);

            if (!deleted)
                return NotFound(new { message = "Notification not found or you do not have permission to delete it" });

            return Ok(new { message = "Notification deleted successfully" });
        }

    }
}
