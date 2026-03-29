using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Application.Services
{
    public class AdminFleetService : IAdminFleetService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminFleetService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AdminFleetCarListItem>> LoadCarsAsync()
        {
            var today = DateTime.Today;

            var cars = await _dbContext.Cars
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Rentals)
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .ToListAsync();

            return cars.Select(car =>
            {
                var isRented = car.Rentals.Any(r =>
                    r.Status != "Cancelled" &&
                    r.RentalDate.Date <= today &&
                    r.ReturnDate.Date >= today);

                var status = isRented
                    ? "Rented"
                    : car.IsAvailable ? "Available" : "Unavailable";

                return new AdminFleetCarListItem
                {
                    CarId = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    RegistrationNumber = car.RegistrationNumber,
                    YearOfProduction = car.YearOfProduction,
                    PricePerDay = car.PricePerDay,
                    Mileage = car.Mileage,
                    IsAvailable = car.IsAvailable,
                    CategoryName = car.Category?.Name ?? "N/A",
                    Status = status
                };
            }).ToList();
        }

        public async Task<List<AdminFleetCategoryItem>> LoadCategoriesAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new AdminFleetCategoryItem
                {
                    CategoryId = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<AdminFleetCarFormModel?> GetCarForEditAsync(int carId)
        {
            var car = await _dbContext.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == carId);

            if (car is null)
            {
                return null;
            }

            return new AdminFleetCarFormModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                YearOfProduction = car.YearOfProduction,
                PricePerDay = car.PricePerDay,
                Mileage = car.Mileage,
                IsAvailable = car.IsAvailable,
                CategoryId = car.CategoryId
            };
        }

        public async Task<bool> AddCarAsync(AdminFleetCarFormModel model)
        {
            var normalizedReg = model.RegistrationNumber.Trim().ToUpperInvariant();

            var regExists = await _dbContext.Cars
                .AnyAsync(c => c.RegistrationNumber.ToUpper() == normalizedReg);

            if (regExists)
            {
                return false;
            }

            var car = new Car
            {
                Brand = model.Brand.Trim(),
                Model = model.Model.Trim(),
                RegistrationNumber = normalizedReg,
                YearOfProduction = model.YearOfProduction,
                PricePerDay = model.PricePerDay,
                Mileage = model.Mileage,
                IsAvailable = model.IsAvailable,
                CategoryId = model.CategoryId
            };

            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCarAsync(AdminFleetCarFormModel model)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (car is null)
            {
                return false;
            }

            var normalizedReg = model.RegistrationNumber.Trim().ToUpperInvariant();

            var regExists = await _dbContext.Cars
                .AnyAsync(c => c.Id != model.Id && c.RegistrationNumber.ToUpper() == normalizedReg);

            if (regExists)
            {
                return false;
            }

            car.Brand = model.Brand.Trim();
            car.Model = model.Model.Trim();
            car.RegistrationNumber = normalizedReg;
            car.YearOfProduction = model.YearOfProduction;
            car.PricePerDay = model.PricePerDay;
            car.Mileage = model.Mileage;
            car.IsAvailable = model.IsAvailable;
            car.CategoryId = model.CategoryId;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);
            if (car is null)
            {
                return false;
            }

            var hasRentals = await _dbContext.Rentals.AnyAsync(r => r.CarId == carId);
            if (hasRentals)
            {
                return false;
            }

            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}