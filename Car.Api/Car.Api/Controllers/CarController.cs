using CarRental.App.DTOs;
using CarRental.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarService _carService;

        public CarController(CarService carService)
        {
            _carService = carService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null)
                return NotFound(new { Message = $"Car with ID {id} not found." });

            return Ok(car);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateCarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _carService.AddAsync(dto);
            return Ok(new { Message = "Car added successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _carService.UpdateAsync(dto);
            if (!result)
                return NotFound(new { Message = $"Car with ID {dto.Id} not found." });

            return Ok(new { Message = "Car updated successfully." });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _carService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { Message = $"Car with ID {id} not found." });

                return Ok(new { Message = "Car deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }




        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] FilterDto filter)
        {
            var cars = await _carService.GetByFiltersAsync(filter);
            return Ok(cars);
        }




    }
}
