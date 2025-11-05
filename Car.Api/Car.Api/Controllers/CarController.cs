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

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _carService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            return car == null ? NotFound("Car Not Found") : Ok(car);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarDto car)
        {
            await _carService.AddAsync(car);
            return Ok("Car Created Successfully");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarDto dto)
        {
            var result = await _carService.UpdateAsync(id, dto);
            if (!result) return NotFound($"Car with ID {id} not found");

            return Ok("Car Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _carService.DeleteAsync(id);
            return deleted ? Ok("Deleted Successfully") : NotFound("Car Not Found");
        }



        [HttpPost("filter")]
        public async Task<IActionResult> FilterCars([FromBody] FilterDto filter)
        {
            var result = await _carService.GetByFiltersAsync(filter);
            return Ok(result);
        }










    }
}
