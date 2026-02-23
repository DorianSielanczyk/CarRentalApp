using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using CarRentalApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetCategoryWithCarsAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Cars)
                    .ThenInclude(car => car.CarPhotos)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithCarsAsync()
        {
            return await _dbSet
                .Include(c => c.Cars)
                    .ThenInclude(car => car.CarPhotos)
                .ToListAsync();
        }
    }
}