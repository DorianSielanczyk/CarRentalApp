using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace CarRentalApp.Application.Services
{
    public class MyRentalsService : IMyRentalsService
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public MyRentalsService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<(List<Rental> Rentals, string? ErrorMessage)> LoadRentalsAsync(string userId)
        {
            var client = await _unitOfWork.Clients.GetClientByUserIdAsync(userId);
            if (client is null)
            {
                return (new List<Rental>(), "Client profile not found.");
            }

            var rentals = (await _unitOfWork.Rentals.GetRentalsByClientIdAsync(client.Id))
                .OrderByDescending(r => r.Id)
                .ToList();

            return (rentals, null);
        }

        public async Task<(bool Success, string? ErrorMessage, string? SuccessMessage)> UploadRentalPhotosAsync(
            int rentalId,
            List<IBrowserFile> files,
            string photoType)
        {
            if (files.Count == 0)
            {
                return (false, "Select file first.", null);
            }

            var rental = await _unitOfWork.Rentals.GetByIdAsync(rentalId);
            if (rental is null)
            {
                return (false, "Reservation not found.", null);
            }

            var uploadsDirectory = Path.Combine(_environment.WebRootPath, "images", "rental-checks");
            Directory.CreateDirectory(uploadsDirectory);

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.Name).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return (false, $"File {file.Name} has invalid format.", null);
                }

                var fileName = $"{rentalId}_{photoType}_{Guid.NewGuid():N}{extension}";
                var fullPath = Path.Combine(uploadsDirectory, fileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(stream);

                await _unitOfWork.RentalPhotos.AddAsync(new RentalPhoto
                {
                    RentalId = rentalId,
                    PhotoUrl = $"/images/rental-checks/{fileName}",
                    PhotoType = photoType,
                    UploadedAtUtc = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return (true, null, $"{files.Count} photo(s) uploaded.");
        }

        public async Task<(bool Success, string? ErrorMessage, string? SuccessMessage)> CancelReservationAsync(int rentalId)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(rentalId);
            if (rental is null)
            {
                return (false, "Reservation not found.", null);
            }

            if (!CanCancelReservation(rental))
            {
                return (false, "You can cancel only if booking starts in more than 7 days.", null);
            }

            rental.Status = "Cancelled";
            _unitOfWork.Rentals.Update(rental);
            await _unitOfWork.SaveChangesAsync();

            return (true, null, "Reservation cancelled.");
        }

        private static bool CanCancelReservation(Rental rental)
        {
            return rental.Status == "Active"
                && (rental.RentalDate.Date - DateTime.Today).TotalDays > 7; 
        }
    }
}