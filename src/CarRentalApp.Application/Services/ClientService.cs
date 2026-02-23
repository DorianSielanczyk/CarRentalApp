using CarRentalApp.Application.DTOs;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            return clients.Select(MapToDto);
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            return client != null ? MapToDto(client) : null;
        }

        public async Task<ClientDto?> GetClientByUserIdAsync(string userId)
        {
            var client = await _unitOfWork.Clients.GetClientByUserIdAsync(userId);
            return client != null ? MapToDto(client) : null;
        }

        public async Task<bool> CreateClientAsync(ClientDto clientDto)
        {
            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                DriverLicense = clientDto.DriverLicense,
                UserId = clientDto.UserId
            };

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdateClientAsync(ClientDto clientDto)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientDto.Id);
            if (client == null)
                return false;

            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.DriverLicense = clientDto.DriverLicense;

            _unitOfWork.Clients.Update(client);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        private static ClientDto MapToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                DriverLicense = client.DriverLicense,
                UserId = client.UserId
            };
        }
    }
}