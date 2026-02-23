using CarRentalApp.Application.DTOs;

namespace CarRentalApp.Application.Services
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto?> GetClientByIdAsync(int id);
        Task<ClientDto?> GetClientByUserIdAsync(string userId);
        Task<bool> CreateClientAsync(ClientDto clientDto);
        Task<bool> UpdateClientAsync(ClientDto clientDto);
    }
}