using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.WebUI.Server.Services
{
    public class AdminBreakdownReportsService : IAdminBreakdownReportsService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminBreakdownReportsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BreakdownReport>> LoadReportsAsync()
        {
            return await _dbContext.BreakdownReports
                .Include(r => r.Rental)!.ThenInclude(x => x!.Car)
                .Include(r => r.Rental)!.ThenInclude(x => x!.Client)
                .Include(r => r.Notes)
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task<bool> ChangeStatusAsync(int reportId, BreakdownStatus status)
        {
            var report = await _dbContext.BreakdownReports.FirstOrDefaultAsync(r => r.Id == reportId);
            if (report is null)
            {
                return false;
            }

            report.Status = status;
            report.UpdatedAtUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddNoteAsync(int reportId, string note, string addedBy)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                return false;
            }

            var report = await _dbContext.BreakdownReports.FirstOrDefaultAsync(r => r.Id == reportId);
            if (report is null)
            {
                return false;
            }

            _dbContext.BreakdownReportNotes.Add(new BreakdownReportNote
            {
                BreakdownReportId = reportId,
                Note = note,
                AddedBy = addedBy,
                CreatedAtUtc = DateTime.UtcNow
            });

            report.UpdatedAtUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}