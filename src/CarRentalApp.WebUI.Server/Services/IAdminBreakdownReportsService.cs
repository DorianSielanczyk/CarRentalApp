using CarRentalApp.Domain.Entities.Breakdowns;

namespace CarRentalApp.WebUI.Server.Services
{
    public interface IAdminBreakdownReportsService
    {
        Task<List<BreakdownReport>> LoadReportsAsync();
        Task<bool> ChangeStatusAsync(int reportId, BreakdownStatus status);
        Task<bool> AddNoteAsync(int reportId, string note, string addedBy);
    }
}