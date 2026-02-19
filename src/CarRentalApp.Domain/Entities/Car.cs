using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;   
        public string Model { get; set; } = string.Empty;
        public string RegistrationNumber { get;set; } = string.Empty;   
        public int YearOfProduction { get; set; }
        public decimal PricePerDay { get; set; }
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }

        // Navigation property for Category
        public Category? Category { get; set; } 
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<CarPhoto> CarPhotos { get; set; } = new List<CarPhoto>();
    }
}
