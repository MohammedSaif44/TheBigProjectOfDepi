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

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            var existing = await _context.Cars.FindAsync(car.Id);
            if (existing == null) return false;
            
            existing.Make = car.Make;
            existing.Model = car.Model;
            existing.Year = car.Year;
            existing.PricePerDay = car.PricePerDay;
            existing.Status = car.Status;
            existing.ImageUrl = car.ImageUrl;

            _context.Cars.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
                return false;

            if (car.Reservations != null && car.Reservations.Any())
                throw new InvalidOperationException("Cannot delete car with existing reservations.");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Car>> GetByFiltersAsync(string? make, string? model, int? year, decimal? minPrice, decimal? maxPrice, string? status)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(make))
                query = query.Where(c => c.Make.Contains(make));

            if (!string.IsNullOrEmpty(model))
                query = query.Where(c => c.Model.Contains(model));

            if (minPrice.HasValue)
                query = query.Where(c => c.PricePerDay >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (year.HasValue)
                query = query.Where(c => c.Year == year.Value);

            return await query.ToListAsync();
        }

    

    }
}
