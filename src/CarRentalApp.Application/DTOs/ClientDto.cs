namespace CarRentalApp.Application.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string DriverLicense { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}