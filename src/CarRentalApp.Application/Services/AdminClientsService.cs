using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Application.Services
{
    public class AdminClientsService : IAdminClientsService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminClientsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AdminClientListItem>> LoadClientsAsync()
        {
            var clients = await _dbContext.Clients
                .AsNoTracking()
                .Include(c => c.Rentals)
                .ToListAsync();

            var userIds = clients
                .Select(c => c.UserId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Cast<string>()
                .Distinct()
                .ToList();

            var emailsByUserId = await _dbContext.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.Email })
                .ToDictionaryAsync(x => x.Id, x => x.Email ?? string.Empty);

            return clients
                .Select(c => new AdminClientListItem
                {
                    ClientId = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DriverLicense = c.DriverLicense,
                    Email = c.UserId is not null && emailsByUserId.TryGetValue(c.UserId, out var email) ? email : "No email",
                    ReservationCount = c.Rentals.Count
                })
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();
        }

        public async Task<AdminClientDetails?> LoadClientDetailsAsync(int clientId)
        {
            var client = await _dbContext.Clients
                .AsNoTracking()
                .Include(c => c.Rentals)
                    .ThenInclude(r => r.Car)
                .Include(c => c.Rentals)
                    .ThenInclude(r => r.RentalPhotos)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client is null)
            {
                return null;
            }

            var email = string.Empty;
            if (!string.IsNullOrWhiteSpace(client.UserId))
            {
                email = await _dbContext.Users
                    .AsNoTracking()
                    .Where(u => u.Id == client.UserId)
                    .Select(u => u.Email ?? string.Empty)
                    .FirstOrDefaultAsync();
            }

            return new AdminClientDetails
            {
                ClientId = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                DriverLicense = client.DriverLicense,
                Email = string.IsNullOrWhiteSpace(email) ? "No email" : email,
                Reservations = client.Rentals
                    .OrderByDescending(r => r.Id)
                    .Select(r => new AdminClientReservationItem
                    {
                        RentalId = r.Id,
                        RentalDate = r.RentalDate,
                        ReturnDate = r.ReturnDate,
                        Status = r.Status,
                        TotalCost = r.TotalCost,
                        CarBrand = r.Car?.Brand ?? string.Empty,
                        CarModel = r.Car?.Model ?? string.Empty,
                        RegistrationNumber = r.Car?.RegistrationNumber ?? string.Empty,
                        BeforePhotos = r.RentalPhotos
                            .Where(p => p.PhotoType == "BeforePickup")
                            .Select(p => p.PhotoUrl)
                            .ToList(),
                        AfterPhotos = r.RentalPhotos
                            .Where(p => p.PhotoType == "AfterReturn")
                            .Select(p => p.PhotoUrl)
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}