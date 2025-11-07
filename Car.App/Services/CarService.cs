using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Services
{
    public class CarService
    {
        private readonly ICarRepository _repository;

        public CarService(ICarRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            var cars = await _repository.GetAllAsync();
            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                PricePerDay = c.PricePerDay,
                Status = c.Status
            }).ToList();
        }

        public async Task<CarDto?> GetByIdAsync(int id)
        {
            var car = await _repository.GetByIdAsync(id);
            if (car == null) return null;

            return new CarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                PricePerDay = car.PricePerDay,
                Status = car.Status
            };
        }

        public async Task AddAsync(CreateCarDto dto)
        {
            var car = new Car
            {
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                PricePerDay = dto.PricePerDay,
                Status = "Available"
            };

            await _repository.AddAsync(car);
        }

        public async Task<bool> UpdateAsync(UpdateCarDto dto)
        {
            var car = new Car
            {
                Id = dto.Id,
                Make = dto.Make ?? "",
                Model = dto.Model ?? "",
                Year = dto.Year,
                PricePerDay = dto.PricePerDay,
                Status = dto.Status ?? "Available"
            };

            return await _repository.UpdateAsync(car);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CarDto>> GetByFiltersAsync(FilterDto filter)
        {
            var cars = await _repository.GetByFiltersAsync(
                filter.Make,
                filter.Model,
                 filter.Year,
                filter.MinPrice,
                filter.MaxPrice,
                filter.Status
            );

            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                PricePerDay = c.PricePerDay,
                Status = c.Status
            }).ToList();
        }



    }
}
