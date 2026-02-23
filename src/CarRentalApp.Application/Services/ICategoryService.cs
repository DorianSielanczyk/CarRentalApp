using CarRentalApp.Application.DTOs;

namespace CarRentalApp.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto?> GetCategoryWithCarsAsync(int id);
    }
}