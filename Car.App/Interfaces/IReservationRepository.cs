using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId);
        Task<Reservation?> GetByIdAsync(int id);
        Task AddAsync(Reservation reservation);
        Task<bool> UpdateAsync(Reservation reservation);
        Task<bool> DeleteAsync(int id);
    }

}
