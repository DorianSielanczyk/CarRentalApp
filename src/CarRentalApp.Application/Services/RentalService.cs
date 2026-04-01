using CarRentalApp.Application.DTOs;
using CarRentalApp.Application.InterfacesDTO;
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
            var car = await _unitOfWork.Cars.GetByIdAsync(rentalDto.CarId);
            if (car == null)
                throw new InvalidOperationException("Car not found");

            if (!car.IsAvailable)
                throw new InvalidOperationException("This car is unavailable");

            var existingRentals = await _unitOfWork.Rentals.GetRentalsByCarIdAsync(rentalDto.CarId);
            var hasOverlap = existingRentals.Any(r =>
                r.Status != "Cancelled" &&
                r.RentalDate.Date <= rentalDto.ReturnDate.Date &&
                r.ReturnDate.Date >= rentalDto.RentalDate.Date);

            if (hasOverlap)
                throw new InvalidOperationException("Selected dates are not available");

            var days = (rentalDto.ReturnDate - rentalDto.RentalDate).Days + 1;
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

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RentalDto>> GetRentalsByCarIdAsync(int carId)
        {
            var rentals = await _unitOfWork.Rentals.GetRentalsByCarIdAsync(carId);
            return rentals.Select(MapToDto);
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
                ClientName = rental.Client != null ? $"{rental.Client.FirstName} {rental.Client.LastName}" : string.Empty,
                MainPhotoUrl = rental.Car.CarPhotos?.FirstOrDefault(p => p.IsMain)?.PhotoUrl ?? "/images/cars/default.jpg",
                PhotoUrls = rental.Car.CarPhotos?.Select(p => p.PhotoUrl).ToList() ?? new List<string>()
            };
        }
    }
}