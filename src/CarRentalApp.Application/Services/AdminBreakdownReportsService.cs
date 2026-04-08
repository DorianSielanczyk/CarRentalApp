using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class AdminBreakdownReportsService : IAdminBreakdownReportsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminBreakdownReportsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BreakdownReport>> LoadReportsAsync()
        {
            return await _unitOfWork.BreakdownReports.GetAllWithDetailsAsync();
        }

        public async Task<bool> ChangeStatusAsync(int reportId, BreakdownStatus status)
        {
            var report = await _unitOfWork.BreakdownReports.GetByIdAsync(reportId);
            if (report is null)
            {
                return false;
            }

            report.Status = status;
            report.UpdatedAtUtc = DateTime.UtcNow;

            _unitOfWork.BreakdownReports.Update(report);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddNoteAsync(int reportId, string note, string addedBy)
        {
            if (string.IsNullOrWhiteSpace(note))
            {
                return false;
            }

            var report = await _unitOfWork.BreakdownReports.GetByIdAsync(reportId);
            if (report is null)
            {
                return false;
            }

            await _unitOfWork.BreakdownReportNotes.AddAsync(new BreakdownReportNote
            {
                BreakdownReportId = reportId,
                Note = note,
                AddedBy = addedBy,
                CreatedAtUtc = DateTime.UtcNow
            });

            report.UpdatedAtUtc = DateTime.UtcNow;
            _unitOfWork.BreakdownReports.Update(report);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}