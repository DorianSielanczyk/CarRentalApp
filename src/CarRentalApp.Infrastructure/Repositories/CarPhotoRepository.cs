using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class CarPhotoRepository : Repository<CarPhoto>, ICarPhotoRepository
    {
        public CarPhotoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CarPhoto>> GetPhotosByCarIdAsync(int carId)
        {
            return await _dbSet
                .Where(cp => cp.CarId == carId)
                .OrderByDescending(cp => cp.IsMain)
                .ToListAsync();
        }

        public async Task<CarPhoto?> GetMainPhotoByCarIdAsync(int carId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(cp => cp.CarId == carId && cp.IsMain);
        }
    }
}