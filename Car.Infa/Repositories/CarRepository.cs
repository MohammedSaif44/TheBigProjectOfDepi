using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using CarRental.Core.Entities;
using CarRental.Infa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infa.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            return await _context.Cars
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    PricePerDay = c.PricePerDay,
                    Status = c.Status
                }).ToListAsync();
        }
        public async Task<CarDto> GetByIdAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
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
        public async Task AddAsync(CarDto carDto)
        {
            var car = new Car
            {
                Make = carDto.Make,
                Model = carDto.Model,
                Year = carDto.Year,
                PricePerDay = carDto.PricePerDay,
                Status = carDto.Status
            };

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(int id, UpdateCarDto dto)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;

            car.Make = dto.Make;
            car.Model = dto.Model;
            car.Year = dto.Year;
            car.PricePerDay = dto.PricePerDay;
            car.Status = dto.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }






        public async Task<IEnumerable<CarDto>> GetByFiltersAsync(FilterDto filter)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Make))
                query = query.Where(c => c.Make.Contains(filter.Make));

            if (!string.IsNullOrWhiteSpace(filter.Model))
                query = query.Where(c => c.Model.Contains(filter.Model));

            if (filter.Year.HasValue)
                query = query.Where(c => c.Year == filter.Year);



            return await query
                .Select(c => new CarDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year,
                    Status = c.Status,

                })
                .ToListAsync();
        }




































    }
}
