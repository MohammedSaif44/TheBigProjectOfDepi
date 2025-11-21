using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using Microsoft.AspNetCore.Http;
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
                Status = c.Status,
                ImageUrl = c.ImageUrl
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
                Status = car.Status,
                ImageUrl = car.ImageUrl
            };
        }
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var folder = Path.Combine("wwwroot", "images", "cars");
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/cars/{fileName}";
        }
        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(rootPath, imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }


        public async Task AddAsync(CreateCarDto dto)
        {
            string? imageUrl = null;

            if (dto.Image != null)
                imageUrl = await SaveImageAsync(dto.Image);

            var car = new Car
            {
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                PricePerDay = dto.PricePerDay,
                Status = "Available",
                ImageUrl = imageUrl
            };

            await _repository.AddAsync(car);
        }




        public async Task<bool> UpdateAsync(UpdateCarDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null) return false;

            if (dto.Image != null)
            {
                DeleteImage(existing.ImageUrl);
                existing.ImageUrl = await SaveImageAsync(dto.Image);
            }

            existing.Make = dto.Make;
            existing.Model = dto.Model;
            existing.Year = dto.Year;
            existing.PricePerDay = dto.PricePerDay;
            existing.Status = dto.Status;

            return await _repository.UpdateAsync(existing);
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
                Status = c.Status,
                ImageUrl = c.ImageUrl
            }).ToList();
        }



    }
}
