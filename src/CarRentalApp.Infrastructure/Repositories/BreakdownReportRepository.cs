using CarRentalApp.Domain.Entities.Breakdowns;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class BreakdownReportRepository : Repository<BreakdownReport>, IBreakdownReportRepository
    {
        public BreakdownReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<BreakdownReport>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(r => r.Rental)!.ThenInclude(x => x!.Car)
                .Include(r => r.Rental)!.ThenInclude(x => x!.Client)
                .Include(r => r.Notes)
                .OrderByDescending(r => r.CreatedAtUtc)
                .ToListAsync();
        }
    }
}