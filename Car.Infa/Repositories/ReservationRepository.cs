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
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Reservation reservation)
        {
            var exist = await _context.Reservations.FindAsync(reservation.Id);
            if (exist == null)
                return false;

            exist.StartDate = reservation.StartDate;
            exist.EndDate = reservation.EndDate;
            exist.Status = reservation.Status;
            exist.TotalPrice = reservation.TotalPrice;

            _context.Reservations.Update(exist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
