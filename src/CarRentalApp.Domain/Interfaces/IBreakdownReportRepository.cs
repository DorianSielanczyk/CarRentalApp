using CarRentalApp.Domain.Entities.Breakdowns;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IBreakdownReportRepository : IRepository<BreakdownReport>
    {
        Task<List<BreakdownReport>> GetAllWithDetailsAsync();
    }
}