using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class AdminFleetService : IAdminFleetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminFleetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AdminFleetCarListItem>> LoadCarsAsync()
        {
            var today = DateTime.Today;

            var cars = (await _unitOfWork.Cars.GetAllAsync())
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .ToList();

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
                    CategoryId = car.CategoryId,
                    MainPhotoUrl = car.CarPhotos
                        .OrderByDescending(p => p.IsMain)
                        .ThenBy(p => p.Id)
                        .Select(p => p.PhotoUrl)
                        .FirstOrDefault() ?? string.Empty,
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
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories
                .OrderBy(c => c.Name)
                .Select(c => new AdminFleetCategoryItem
                {
                    CategoryId = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public async Task<AdminFleetCarFormModel?> GetCarForEditAsync(int carId)
        {
            var car = await _unitOfWork.Cars.GetCarWithDetailsAsync(carId);
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
                CategoryId = car.CategoryId,
                MainPhotoUrl = car.CarPhotos
                    .OrderByDescending(p => p.IsMain)
                    .ThenBy(p => p.Id)
                    .Select(p => p.PhotoUrl)
                    .FirstOrDefault() ?? string.Empty,
                ExistingPhotos = car.CarPhotos
                    .OrderByDescending(p => p.IsMain)
                    .ThenBy(p => p.Id)
                    .Select(p => new AdminFleetCarPhotoItem
                    {
                        Id = p.Id,
                        PhotoUrl = p.PhotoUrl,
                        IsMain = p.IsMain
                    })
                    .ToList()
            };
        }

        public async Task<bool> AddCarAsync(AdminFleetCarFormModel model)
        {
            var normalizedReg = model.RegistrationNumber.Trim().ToUpperInvariant();

            var regExists = await _unitOfWork.Cars
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

            var mainPhotoUrl = model.MainPhotoUrl.Trim();
            if (!string.IsNullOrWhiteSpace(mainPhotoUrl))
            {
                car.CarPhotos.Add(new CarPhoto
                {
                    PhotoUrl = mainPhotoUrl,
                    IsMain = true
                });
            }

            foreach (var additionalPhotoUrl in ParseAdditionalPhotoUrls(model.AdditionalPhotoUrls))
            {
                if (string.Equals(additionalPhotoUrl, mainPhotoUrl, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                car.CarPhotos.Add(new CarPhoto
                {
                    PhotoUrl = additionalPhotoUrl,
                    IsMain = false
                });
            }

            await _unitOfWork.Cars.AddAsync(car);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCarAsync(AdminFleetCarFormModel model)
        {
            var car = await _unitOfWork.Cars.GetCarWithDetailsAsync(model.Id);
            if (car is null)
            {
                return false;
            }

            var normalizedReg = model.RegistrationNumber.Trim().ToUpperInvariant();

            var regExists = await _unitOfWork.Cars
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

            if (model.DeletedPhotoIds.Count > 0)
            {
                var photosToDelete = car.CarPhotos
                    .Where(p => model.DeletedPhotoIds.Contains(p.Id))
                    .ToList();

                if (photosToDelete.Count > 0)
                {
                    _unitOfWork.CarPhotos.RemoveRange(photosToDelete);

                    foreach (var photo in photosToDelete)
                    {
                        car.CarPhotos.Remove(photo);
                    }
                }
            }

            var mainPhotoUrl = model.MainPhotoUrl.Trim();
            if (!string.IsNullOrWhiteSpace(mainPhotoUrl))
            {
                var mainPhoto = car.CarPhotos
                    .Where(p => !model.DeletedPhotoIds.Contains(p.Id))
                    .OrderByDescending(p => p.IsMain)
                    .ThenBy(p => p.Id)
                    .FirstOrDefault();

                if (mainPhoto is null)
                {
                    car.CarPhotos.Add(new CarPhoto
                    {
                        PhotoUrl = mainPhotoUrl,
                        IsMain = true
                    });
                }
                else
                {
                    mainPhoto.PhotoUrl = mainPhotoUrl;
                    mainPhoto.IsMain = true;

                    foreach (var photo in car.CarPhotos.Where(p => p.Id != mainPhoto.Id))
                    {
                        photo.IsMain = false;
                    }
                }
            }

            foreach (var additionalPhotoUrl in ParseAdditionalPhotoUrls(model.AdditionalPhotoUrls))
            {
                if (car.CarPhotos
                    .Where(p => !model.DeletedPhotoIds.Contains(p.Id))
                    .Any(p => string.Equals(p.PhotoUrl, additionalPhotoUrl, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                car.CarPhotos.Add(new CarPhoto
                {
                    PhotoUrl = additionalPhotoUrl,
                    IsMain = false
                });
            }

            if (car.CarPhotos.Any() && !car.CarPhotos.Any(p => p.IsMain))
            {
                car.CarPhotos.OrderBy(p => p.Id).First().IsMain = true;
            }

            _unitOfWork.Cars.Update(car);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            var car = await _unitOfWork.Cars.GetByIdAsync(carId);
            if (car is null)
            {
                return false;
            }

            var hasRentals = await _unitOfWork.Rentals.AnyAsync(r => r.CarId == carId);
            if (hasRentals)
            {
                return false;
            }

            _unitOfWork.Cars.Remove(car);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private static IEnumerable<string> ParseAdditionalPhotoUrls(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return [];
            }

            return value
                .Split([',', ';', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}