using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Domain.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace CarRentalApp.Application.Services
{
    public class ReportDamageService : IReportDamageService
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public ReportDamageService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<(List<Rental> ActiveRentals, List<BreakdownReport> History)> LoadDataAsync(string userId)
        {
            var client = await _unitOfWork.Clients.GetClientByUserIdAsync(userId);
            if (client is null)
            {
                return (new List<Rental>(), new List<BreakdownReport>());
            }

            var today = DateTime.Today;

            var activeRentals = (await _unitOfWork.Rentals.GetRentalsByClientIdAsync(client.Id))
                .Where(r =>
                    r.Status == "Active" &&
                    r.RentalDate.Date <= today &&
                    r.ReturnDate.Date >= today)
                .OrderByDescending(r => r.Id)
                .ToList();

            var history = (await _unitOfWork.BreakdownReports.GetAllWithDetailsAsync())
                .Where(r => r.Rental?.ClientId == client.Id)
                .ToList();

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
            var client = await _unitOfWork.Clients.GetClientByUserIdAsync(userId);
            if (client is null)
            {
                return (false, "Client profile not found.");
            }

            var today = DateTime.Today;

            var selectedRental = (await _unitOfWork.Rentals.GetRentalsByClientIdAsync(client.Id))
                .FirstOrDefault(r =>
                    r.Id == selectedRentalId &&
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

            await _unitOfWork.BreakdownReports.AddAsync(report);
            await _unitOfWork.SaveChangesAsync();

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

                report.Photos.Add(new BreakdownReportPhoto
                {
                    BreakdownReportId = report.Id,
                    PhotoUrl = $"/images/breakdowns/{name}",
                    UploadedAtUtc = DateTime.UtcNow
                });
            }

            _unitOfWork.BreakdownReports.Update(report);
            await _unitOfWork.SaveChangesAsync();

            return (true, null);
        }
    }
}