using CarRentalApp.Application.DTOs;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RentalDto>> GetAllRentalsAsync()
        {
            var rentals = await _unitOfWork.Rentals.GetAllAsync();
            return rentals.Select(MapToDto);
        }

        public async Task<RentalDto?> GetRentalByIdAsync(int id)
        {
            var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id);
            return rental != null ? MapToDto(rental) : null;
        }

        public async Task<IEnumerable<RentalDto>> GetRentalsByClientIdAsync(int clientId)
        {
            var rentals = await _unitOfWork.Rentals.GetRentalsByClientIdAsync(clientId);
            return rentals.Select(MapToDto);
        }

        public async Task<IEnumerable<RentalDto>> GetActiveRentalsAsync()
        {
            var rentals = await _unitOfWork.Rentals.GetActiveRentalsAsync();
            return rentals.Select(MapToDto);
        }

        public async Task<int> CreateRentalAsync(CreateRentalDto rentalDto)
        {
            // Business logic: Verify car is available
            var car = await _unitOfWork.Cars.GetByIdAsync(rentalDto.CarId);
            if (car == null || !car.IsAvailable)
                throw new InvalidOperationException("Car is not available for rental");

            // Business logic: Calculate total cost
            var days = (rentalDto.ReturnDate - rentalDto.RentalDate).Days;
            var totalCost = days * car.PricePerDay;

            var rental = new Rental
            {
                CarId = rentalDto.CarId,
                ClientId = rentalDto.ClientId,
                RentalDate = rentalDto.RentalDate,
                ReturnDate = rentalDto.ReturnDate,
                TotalCost = totalCost,
                Status = "Active",
                IsPaid = rentalDto.IsPaid
            };

            await _unitOfWork.Rentals.AddAsync(rental);
            
            // Business logic: Mark car as unavailable
            car.IsAvailable = false;
            _unitOfWork.Cars.Update(car);
            
            await _unitOfWork.SaveChangesAsync();
            
            return rental.Id;
        }

        public async Task<bool> CompleteRentalAsync(int rentalId)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(rentalId);
            if (rental == null)
                return false;

            rental.Status = "Completed";
            _unitOfWork.Rentals.Update(rental);

            // Make car available again
            var car = await _unitOfWork.Cars.GetByIdAsync(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
                _unitOfWork.Cars.Update(car);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelRentalAsync(int rentalId)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(rentalId);
            if (rental == null)
                return false;

            rental.Status = "Cancelled";
            _unitOfWork.Rentals.Update(rental);

            // Make car available again
            var car = await _unitOfWork.Cars.GetByIdAsync(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
                _unitOfWork.Cars.Update(car);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static RentalDto MapToDto(Rental rental)
        {
            return new RentalDto
            {
                Id = rental.Id,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate,
                TotalCost = rental.TotalCost,
                Status = rental.Status,
                IsPaid = rental.IsPaid,
                CarId = rental.CarId,
                CarBrand = rental.Car?.Brand ?? string.Empty,
                CarModel = rental.Car?.Model ?? string.Empty,
                ClientId = rental.ClientId,
                ClientName = rental.Client != null ? $"{rental.Client.FirstName} {rental.Client.LastName}" : string.Empty
            };
        }
    }
}