using CarRental.App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.App.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarDto>> GetAllAsync();
        Task<CarDto> GetByIdAsync(int id);
        Task AddAsync(CarDto car);
        Task<bool> UpdateAsync(int id, UpdateCarDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CarDto>> GetByFiltersAsync(FilterDto filter);













    }
}
