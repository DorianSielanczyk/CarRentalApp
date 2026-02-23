using CarRentalApp.Application.DTOs;

namespace CarRentalApp.Application.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalDto>> GetAllRentalsAsync();
        Task<RentalDto?> GetRentalByIdAsync(int id);
        Task<IEnumerable<RentalDto>> GetRentalsByClientIdAsync(int clientId);
        Task<IEnumerable<RentalDto>> GetActiveRentalsAsync();
        Task<int> CreateRentalAsync(CreateRentalDto rentalDto);
        Task<bool> CompleteRentalAsync(int rentalId);
        Task<bool> CancelRentalAsync(int rentalId);
    }
}