using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class RentalPhotoRepository : Repository<RentalPhoto>, IRentalPhotoRepository
    {
        public RentalPhotoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RentalPhoto>> GetPhotosByRentalIdAsync(int rentalId)
        {
            return await _dbSet
                .Where(p => p.RentalId == rentalId)
                .OrderByDescending(p => p.UploadedAtUtc)
                .ToListAsync();
        }
    }
}