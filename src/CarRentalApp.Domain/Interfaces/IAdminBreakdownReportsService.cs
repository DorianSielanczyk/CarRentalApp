using CarRentalApp.Domain.Entities.Breakdowns;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IAdminBreakdownReportsService
    {
        Task<List<BreakdownReport>> LoadReportsAsync();
        Task<bool> ChangeStatusAsync(int reportId, BreakdownStatus status);
        Task<bool> AddNoteAsync(int reportId, string note, string addedBy);
    }
}