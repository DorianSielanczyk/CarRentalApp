namespace CarRentalApp.Application.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        
        // Car information
        public int CarId { get; set; }
        public string CarBrand { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public string CarFullName => $"{CarBrand} {CarModel}";
        
        // Client information
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        
        // Calculated properties
        public int RentalDays => (ReturnDate - RentalDate).Days;
        public bool IsActive => Status == "Active";
    }
}