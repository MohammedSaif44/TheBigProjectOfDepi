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
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly ICarRepository _carRepo;

        public ReservationService(IReservationRepository reservationRepo, ICarRepository carRepo)
        {
            _reservationRepo = reservationRepo;
            _carRepo = carRepo;
        }

       
        public async Task AddAsync(string userId, CreateReservationDto dto)
        {
            var car = await _carRepo.GetByIdAsync(dto.CarId);
            if (car == null || car.Status != "Available")
                throw new Exception("Car is not available.");

            if (dto.EndDate <= dto.StartDate)
                throw new Exception("Invalid reservation dates.");

            var days = (dto.EndDate - dto.StartDate).Days;
            var total = car.PricePerDay * days;

            var reservation = new Reservation
            {
                UserId = userId,
                CarId = dto.CarId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = total,
                Status = "Pending"
            };

            await _reservationRepo.AddAsync(reservation);
        }

        
        public async Task<IEnumerable<ReservationDto>> GetByUserAsync(string userId)
        {
            var list = await _reservationRepo.GetByUserIdAsync(userId);

            return list.Select(r => new ReservationDto
            {
                Id = r.Id,
                UserEmail = r.User.Email,
                CarModel = r.Car.Model,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                StartDate = r.StartDate,
                EndDate = r.EndDate
            });
        }

       
        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var list = await _reservationRepo.GetAllAsync();

            return list.Select(r => new ReservationDto
            {
                Id = r.Id,
                UserEmail = r.User.Email,
                CarModel = r.Car.Model,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                StartDate = r.StartDate,
                EndDate = r.EndDate
            });
        }

        
        public async Task<bool> UpdateAsync(UpdateReservationDto dto, string userId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(dto.Id);
            if (reservation == null)
                return false;

            
            if (reservation.UserId != userId)
                throw new Exception("Not allowed.");

            var days = (dto.EndDate - dto.StartDate).Days;
            reservation.StartDate = dto.StartDate;
            reservation.EndDate = dto.EndDate;
            reservation.Status = dto.Status;
            reservation.TotalPrice = reservation.Car.PricePerDay * days;

            return await _reservationRepo.UpdateAsync(reservation);
        }

       
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var res = await _reservationRepo.GetByIdAsync(id);
            if (res == null) return false;

            if (res.UserId != userId)
                throw new Exception("Not allowed.");

            return await _reservationRepo.DeleteAsync(id);
        }
    }

}
