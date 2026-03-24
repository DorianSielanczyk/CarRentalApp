using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.WebUI.Server.Services
{
    public class ReportDamageService : IReportDamageService
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public ReportDamageService(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<(List<Rental> ActiveRentals, List<BreakdownReport> History)> LoadDataAsync(string userId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client is null)
            {
                return (new List<Rental>(), new List<BreakdownReport>());
            }

            var today = DateTime.Today;

            var activeRentals = await _dbContext.Rentals
                .Include(r => r.Car)
                .Where(r =>
                    r.ClientId == client.Id &&
                    r.Status == "Active" &&
                    r.RentalDate.Date <= today &&
                    r.ReturnDate.Date >= today)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            var history = await _dbContext.BreakdownReports
                .Include(r => r.Rental)
                .Include(r => r.Notes)
                .Where(r => r.Rental!.ClientId == client.Id)
                .ToListAsync();

            return (activeRentals, history);
        }

        public async Task<(bool Success, string? Error)> SubmitAsync(
            string userId,
            int selectedRentalId,
            BreakdownType breakdownType,
            string description,
            string locationText,
            decimal? latitude,
            decimal? longitude,
            IReadOnlyList<IBrowserFile> files)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client is null)
            {
                return (false, "Client profile not found.");
            }

            var today = DateTime.Today;

            var selectedRental = await _dbContext.Rentals
                .FirstOrDefaultAsync(r =>
                    r.Id == selectedRentalId &&
                    r.ClientId == client.Id &&
                    r.Status == "Active" &&
                    r.RentalDate.Date <= today &&
                    r.ReturnDate.Date >= today);

            if (selectedRental is null)
            {
                return (false, "Please select a rented car.");
            }

            var report = new BreakdownReport
            {
                RentalId = selectedRental.Id,
                BreakdownType = breakdownType,
                Description = description,
                LocationText = locationText,
                Latitude = latitude,
                Longitude = longitude,
                Status = BreakdownStatus.New,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            _dbContext.BreakdownReports.Add(report);
            await _dbContext.SaveChangesAsync();

            var dir = Path.Combine(_environment.WebRootPath, "images", "breakdowns");
            Directory.CreateDirectory(dir);

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.Name).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext))
                {
                    return (false, $"File {file.Name} has invalid format.");
                }

                var name = $"{report.Id}_{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(dir, name);

                await using var fs = new FileStream(fullPath, FileMode.Create);
                await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(fs);

                _dbContext.BreakdownReportPhotos.Add(new BreakdownReportPhoto
                {
                    BreakdownReportId = report.Id,
                    PhotoUrl = $"/images/breakdowns/{name}",
                    UploadedAtUtc = DateTime.UtcNow
                });
            }

            await _dbContext.SaveChangesAsync();
            return (true, null);
        }
    }
}