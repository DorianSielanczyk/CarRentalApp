namespace CarRentalApp.Application.DTOs
{
    public class UpdateCarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public int YearOfProduction { get; set; }
        public decimal PricePerDay { get; set; }
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
    }
}