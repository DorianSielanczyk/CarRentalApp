namespace CarRentalApp.Domain.Entities
{
    public class RentalPhoto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string PhotoType { get; set; } = string.Empty; 
        public DateTime UploadedAtUtc { get; set; }

        public Rental? Rental { get; set; }
    }
}