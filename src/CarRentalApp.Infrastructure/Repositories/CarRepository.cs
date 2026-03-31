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

        public override async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Include(c => c.Rentals)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            var today = DateTime.Today;

            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Include(c => c.Rentals)
                .Where(c =>
                    c.IsAvailable &&
                    !c.Rentals.Any(r =>
                        r.Status != "Cancelled" &&
                        r.RentalDate.Date <= today &&
                        r.ReturnDate.Date >= today))
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Include(c => c.Rentals)
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Include(c => c.Category)
                .Include(c => c.CarPhotos)
                .Include(c => c.Rentals)
                .Where(c => c.PricePerDay >= minPrice && c.PricePerDay <= maxPrice)
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
    }
}