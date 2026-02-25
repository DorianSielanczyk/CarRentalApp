using CarRentalApp.Application.DTOs;
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
            try
            {
                var cars = await _unitOfWork.Cars.GetAvailableCarsAsync();
                var carsList = cars.ToList();
                
                Console.WriteLine($"CarService: Retrieved {carsList.Count} available cars from repository");
                
                if (carsList.Count > 0)
                {
                    var firstCar = carsList.First();
                    Console.WriteLine($"First car: {firstCar.Brand} {firstCar.Model}");
                    Console.WriteLine($"Category loaded: {firstCar.Category != null}");
                    Console.WriteLine($"Photos count: {firstCar.CarPhotos?.Count ?? 0}");
                }
                
                var dtos = carsList.Select(MapToDto).ToList();
                Console.WriteLine($"CarService: Mapped to {dtos.Count} DTOs");
                
                return dtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CarService ERROR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetCarWithDetailsAsync(id);
            return car != null ? MapToDto(car) : null;
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
            if (car == null)
                return false;

            // Update properties
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
            if (car == null)
                return false;

            car.IsAvailable = isAvailable;
            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(id);
            if (car == null)
                return false;

            _unitOfWork.Cars.Remove(car);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        // Helper method to map Entity to DTO
        private static CarDto MapToDto(Car car)
        {
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
                CategoryId = car.CategoryId,
                CategoryName = car.Category?.Name ?? string.Empty,
                MainPhotoUrl = car.CarPhotos.FirstOrDefault(p => p.IsMain)?.PhotoUrl ?? "/images/cars/default.jpg",
                PhotoUrls = car.CarPhotos.Select(p => p.PhotoUrl).ToList()
            };
        }
    }
}