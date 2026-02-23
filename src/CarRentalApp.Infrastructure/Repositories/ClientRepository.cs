using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Client?> GetClientByUserIdAsync(string userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Client?> GetClientWithRentalsAsync(int clientId)
        {
            return await _dbSet
                .Include(c => c.Rentals)
                    .ThenInclude(r => r.Car)
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Client?> GetClientByDriverLicenseAsync(string driverLicense)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.DriverLicense == driverLicense);
        }
    }
}