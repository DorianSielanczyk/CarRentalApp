using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; } 
        public string Status { get; set; } = string.Empty;
        public bool IsPaid { get; set; }

        // Foreign key for Car
        public int CarId { get; set; }

        // Foreign key for Client
        public int ClientId { get; set; }

        // Navigation properties
        public Car? Car { get; set; }
        public Client? Client { get; set; }
    }
}
