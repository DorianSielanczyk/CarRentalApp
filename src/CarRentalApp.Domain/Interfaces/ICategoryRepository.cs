using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetCategoryWithCarsAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoriesWithCarsAsync();
    }
}
