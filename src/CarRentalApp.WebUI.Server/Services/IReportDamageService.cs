using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Entities.Breakdowns;
using Microsoft.AspNetCore.Components.Forms;

namespace CarRentalApp.WebUI.Server.Services
{
    public interface IReportDamageService
    {
        Task<(List<Rental> ActiveRentals, List<BreakdownReport> History)> LoadDataAsync(string userId);
        Task<(bool Success, string? Error)> SubmitAsync(
            string userId,
            int selectedRentalId,
            BreakdownType breakdownType,
            string description,
            string locationText,
            decimal? latitude,
            decimal? longitude,
            IReadOnlyList<IBrowserFile> files);
    }
}