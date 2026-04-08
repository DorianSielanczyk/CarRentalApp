using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class BreakdownReportNoteRepository : Repository<BreakdownReportNote>, IBreakdownReportNoteRepository
    {
        public BreakdownReportNoteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}