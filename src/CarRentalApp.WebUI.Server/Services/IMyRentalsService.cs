using CarRentalApp.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace CarRentalApp.WebUI.Server.Services
{
    public interface IMyRentalsService
    {
        Task<(List<Rental> Rentals, string? ErrorMessage)> LoadRentalsAsync(string userId);

        Task<(bool Success, string? ErrorMessage, string? SuccessMessage)> UploadRentalPhotosAsync(
            int rentalId,
            List<IBrowserFile> files,
            string photoType);

        Task<(bool Success, string? ErrorMessage, string? SuccessMessage)> CancelReservationAsync(int rentalId);
    }
}