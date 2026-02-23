namespace CarRentalApp.Application.DTOs
{
    public class CreateRentalDto
    {
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsPaid { get; set; }
    }
}