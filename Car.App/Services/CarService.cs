using CarRental.App.DTOs;
using CarRental.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Services
{
    public class CarService
    {
        private readonly ICarRepository _repo;

        public CarService(ICarRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<CarDto>> GetAllAsync() => _repo.GetAllAsync();
        public Task<CarDto> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }
        public Task AddAsync(CarDto car) => _repo.AddAsync(car);

        public Task<bool> UpdateAsync(int id, UpdateCarDto dto)
        {
            return _repo.UpdateAsync(id, dto);
        }
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);


        public Task<IEnumerable<CarDto>> GetByFiltersAsync(FilterDto filter)
                                        => _repo.GetByFiltersAsync(filter);




    }
}
