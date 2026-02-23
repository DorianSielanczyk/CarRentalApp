using CarRentalApp.Application.DTOs;

namespace CarRentalApp.Application.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        Task<IEnumerable<CarDto>> GetAvailableCarsAsync();
        Task<CarDto?> GetCarByIdAsync(int id);
        Task<IEnumerable<CarDto>> GetCarsByCategoryAsync(int categoryId);
        Task<IEnumerable<CarDto>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<bool> UpdateCarAsync(UpdateCarDto carDto);
        Task<bool> UpdateCarAvailabilityAsync(int carId, bool isAvailable);
        Task<bool> DeleteCarAsync(int id);
    }
}