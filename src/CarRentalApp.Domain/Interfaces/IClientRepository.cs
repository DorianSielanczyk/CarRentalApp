using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetClientByUserIdAsync(string userId);
        Task<Client?> GetClientWithRentalsAsync(int clientId);
        Task<Client?> GetClientByDriverLicenseAsync(string driverLicense);
    }
}