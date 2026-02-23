using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Where(c => c.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Car?> GetCarWithDetailsAsync(int carId)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Include(c => c.Rentals)
                .FirstOrDefaultAsync(c => c.Id == carId);
        }

        public async Task<IEnumerable<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Where(c => c.PricePerDay >= minPrice && c.PricePerDay <= maxPrice)
                .ToListAsync();
        }
    }
}