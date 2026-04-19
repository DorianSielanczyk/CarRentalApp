using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IAdminFleetService
    {
        Task<List<AdminFleetCarListItem>> LoadCarsAsync();
        Task<List<AdminFleetCategoryItem>> LoadCategoriesAsync();
        Task<AdminFleetCarFormModel?> GetCarForEditAsync(int carId);
        Task<bool> AddCarAsync(AdminFleetCarFormModel model);
        Task<bool> UpdateCarAsync(AdminFleetCarFormModel model);
        Task<bool> DeleteCarAsync(int carId, bool isAdmin);
    }

    public sealed class AdminFleetCarListItem
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string MainPhotoUrl { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public int YearOfProduction { get; set; }
        public decimal PricePerDay { get; set; }
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool HasReservationHistory { get; set; }
    }

    public sealed class AdminFleetCategoryItem
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public sealed class AdminFleetCarFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Range(1950, 2200)]
        public int YearOfProduction { get; set; } = DateTime.Now.Year;

        [Range(1, 1000000)]
        public decimal PricePerDay { get; set; }

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public string MainPhotoUrl { get; set; } = string.Empty;

        public string AdditionalPhotoUrls { get; set; } = string.Empty;

        public List<AdminFleetCarPhotoItem> ExistingPhotos { get; set; } = [];

        public List<int> DeletedPhotoIds { get; set; } = [];
    }

    public sealed class AdminFleetCarPhotoItem
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}