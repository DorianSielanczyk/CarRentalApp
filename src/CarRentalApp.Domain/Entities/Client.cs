using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DriverLicense { get; set; } = string.Empty;
        public string? UserId { get; set; }  
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
