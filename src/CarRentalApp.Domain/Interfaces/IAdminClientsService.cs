namespace CarRentalApp.Domain.Interfaces
{
    public interface IAdminClientsService
    {
        Task<List<AdminClientListItem>> LoadClientsAsync();
        Task<AdminClientDetails?> LoadClientDetailsAsync(int clientId);
    }

    public sealed class AdminClientListItem
    {
        public int ClientId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DriverLicense { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public int ReservationCount { get; init; }
    }

    public sealed class AdminClientDetails
    {
        public int ClientId { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DriverLicense { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public List<AdminClientReservationItem> Reservations { get; init; } = [];
    }

    public sealed class AdminClientReservationItem
    {
        public int RentalId { get; init; }
        public DateTime RentalDate { get; init; }
        public DateTime ReturnDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal TotalCost { get; init; }
        public string CarBrand { get; init; } = string.Empty;
        public string CarModel { get; init; } = string.Empty;
        public string RegistrationNumber { get; init; } = string.Empty;
        public List<string> BeforePhotos { get; init; } = [];
        public List<string> AfterPhotos { get; init; } = [];
    }
}