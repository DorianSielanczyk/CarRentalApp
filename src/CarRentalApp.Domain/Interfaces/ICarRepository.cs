using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<IEnumerable<Car>> GetCarsByCategoryAsync(int categoryId);
        Task<Car?> GetCarWithDetailsAsync(int carId);
        Task<IEnumerable<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
