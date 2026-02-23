using CarRentalApp.Application.DTOs;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;

namespace CarRentalApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category != null ? MapToDto(category) : null;
        }

        public async Task<CategoryDto?> GetCategoryWithCarsAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category != null ? MapToDto(category) : null;
        }

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CarCount = category.Cars?.Count ?? 0
            };
        }
    }
}