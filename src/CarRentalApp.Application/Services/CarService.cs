using CarRentalApp.Application.DTOs;
using CarRentalApp.Application.InterfacesDTO;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _unitOfWork.Cars.GetAllAsync();
            return cars.Select(MapToDto);
        }

        public async Task<IEnumerable<CarDto>> GetAvailableCarsAsync()
        {
            var cars = await _unitOfWork.Cars.GetAvailableCarsAsync();
            return cars.Select(MapToDto).ToList();
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetCarWithDetailsAsync(id);
            return car is not null ? MapToDto(car) : null;
        }

        public async Task<IEnumerable<CarDto>> GetCarsByCategoryAsync(int categoryId)
        {
            var cars = await _unitOfWork.Cars.GetCarsByCategoryAsync(categoryId);
            return cars.Select(MapToDto);
        }

        public async Task<IEnumerable<CarDto>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var cars = await _unitOfWork.Cars.GetCarsByPriceRangeAsync(minPrice, maxPrice);
            return cars.Select(MapToDto);
        }

        public async Task<bool> UpdateCarAsync(UpdateCarDto carDto)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(carDto.Id);
            if (car is null)
            {
                return false;
            }

            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.RegistrationNumber = carDto.RegistrationNumber;
            car.YearOfProduction = carDto.YearOfProduction;
            car.PricePerDay = carDto.PricePerDay;
            car.Mileage = carDto.Mileage;
            car.IsAvailable = carDto.IsAvailable;
            car.CategoryId = carDto.CategoryId;

            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCarAvailabilityAsync(int carId, bool isAvailable)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(carId);
            if (car is null)
            {
                return false;
            }

            car.IsAvailable = isAvailable;
            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car is null)
            {
                return false;
            }

            _unitOfWork.Cars.Remove(car);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static CarDto MapToDto(Car car)
        {
            var today = DateTime.Today;
            var isRentedToday = car.Rentals?.Any(r =>
                r.Status != "Cancelled" &&
                r.RentalDate.Date <= today &&
                r.ReturnDate.Date >= today) ?? false;

            var status = !car.IsAvailable
                ? "Unavailable"
                : isRentedToday ? "Rented" : "Available";

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                YearOfProduction = car.YearOfProduction,
                PricePerDay = car.PricePerDay,
                Mileage = car.Mileage,
                IsAvailable = car.IsAvailable,
                Status = status,
                CategoryId = car.CategoryId,
                CategoryName = car.Category?.Name ?? string.Empty,
                MainPhotoUrl = car.CarPhotos.FirstOrDefault(p => p.IsMain)?.PhotoUrl ?? "/images/cars/default.jpg",
                PhotoUrls = car.CarPhotos.Select(p => p.PhotoUrl).ToList()
            };
        }
    }
}