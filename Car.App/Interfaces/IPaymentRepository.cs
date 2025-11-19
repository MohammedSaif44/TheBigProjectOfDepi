using CarRental.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetBySessionIdAsync(string sessionId);
        Task<Payment?> GetByReservationIdAsync(int reservationId);
        Task UpdateAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllAsync();
    }

}
