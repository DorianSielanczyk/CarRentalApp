using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Application.Services
{
    public class MyRentalsService : IMyRentalsService
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public MyRentalsService(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<(List<Rental> Rentals, string? ErrorMessage)> LoadRentalsAsync(string userId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client is null)
            {
                return (new List<Rental>(), "Client profile not found.");
            }

            var rentals = await _dbContext.Rentals
                .Include(r => r.Car)
                    .ThenInclude(c => c!.Category)
                .Include(r => r.Car)
                    .ThenInclude(c => c!.CarPhotos)
                .Include(r => r.RentalPhotos)
                .Where(r => r.ClientId == client.Id)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

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

                _dbContext.RentalPhotos.Add(new RentalPhoto
                {
                    RentalId = rentalId,
                    PhotoUrl = $"/images/rental-checks/{fileName}",
                    PhotoType = photoType,
                    UploadedAtUtc = DateTime.UtcNow
                });
            }

            await _dbContext.SaveChangesAsync();
            return (true, null, $"{files.Count} photo(s) uploaded.");
        }

        public async Task<(bool Success, string? ErrorMessage, string? SuccessMessage)> CancelReservationAsync(int rentalId)
        {
            var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == rentalId);
            if (rental is null)
            {
                return (false, "Reservation not found.", null);
            }

            if (!CanCancelReservation(rental))
            {
                return (false, "You can cancel only if booking starts in more than 7 days.", null);
            }

            rental.Status = "Cancelled";
            await _dbContext.SaveChangesAsync();

            return (true, null, "Reservation cancelled.");
        }

        private static bool CanCancelReservation(Rental rental)
        {
            return rental.Status == "Active"
                && (rental.RentalDate.Date - DateTime.Today).TotalDays > 7;
        }
    }
}