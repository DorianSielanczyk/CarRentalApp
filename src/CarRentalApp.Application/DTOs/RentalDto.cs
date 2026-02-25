namespace CarRentalApp.Application.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public int CarId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string MainPhotoUrl { get; set; } // <-- Add this property
        public List<string> PhotoUrls { get; set; }
    }
}