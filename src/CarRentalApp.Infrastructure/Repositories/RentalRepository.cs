using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Rental?> GetRentalWithDetailsAsync(int rentalId)
        {
            return await _dbSet
                .Include(r => r.Car)
                    .ThenInclude(c => c.Category)
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarPhotos)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.Id == rentalId);
        }

        public async Task<IEnumerable<Rental>> GetRentalsByClientIdAsync(int clientId)
        {
            return await _dbSet
                .Include(r => r.Car)
                    .ThenInclude(c => c.Category)
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarPhotos)
                .Where(r => r.ClientId == clientId)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.Client)
                .Where(r => r.Status == "Active")
                .OrderBy(r => r.ReturnDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId)
        {
            return await _dbSet
                .Include(r => r.Client)
                .Where(r => r.CarId == carId)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();
        }
    }
}