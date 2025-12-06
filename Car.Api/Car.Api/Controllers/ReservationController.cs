using CarRental.App.DTOs;
using CarRental.App.Services;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(ReservationService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        
        [Authorize(Roles = "Customer ,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User ID not found in token.");

            var reservationId = await _service.AddAsync(userId, dto);

            return Ok(new
            {
                Message = "Reservation created successfully.",
                ReservationId = reservationId
            });
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User not found.");

            return Ok(await _service.GetByUserAsync(userId));

        }


        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        
        [Authorize(Roles = "Customer ,Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateReservationDto dto)
        {
            var user = await _userManager.GetUserAsync(User);

            var ok = await _service.UpdateAsync(dto, user.Id);
            if (!ok)
                return NotFound(new { Message = "Reservation not found" });

            return Ok(new { Message = "Reservation updated" });
        }

       
        [Authorize(Roles = "Customer ,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var ok = await _service.DeleteAsync(id, user.Id);

            if (!ok)
                return NotFound(new { Message = "Reservation not found" });

            return Ok(new { success = true, Message = "Reservation deleted" });
        }
    }

}
