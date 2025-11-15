using CarRental.App.DTOs;
using CarRental.App.Services;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        
        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            await _service.AddAsync(user.Id, dto);
            return Ok(new { Message = "Reservation created successfully." });
        }

        
        [Authorize(Roles = "Customer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(await _service.GetByUserAsync(user.Id));
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        
        [Authorize(Roles = "Customer")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateReservationDto dto)
        {
            var user = await _userManager.GetUserAsync(User);

            var ok = await _service.UpdateAsync(dto, user.Id);
            if (!ok)
                return NotFound(new { Message = "Reservation not found" });

            return Ok(new { Message = "Reservation updated" });
        }

       
        [Authorize(Roles = "Customer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var ok = await _service.DeleteAsync(id, user.Id);

            if (!ok)
                return NotFound(new { Message = "Reservation not found" });

            return Ok(new { Message = "Reservation deleted" });
        }
    }

}
