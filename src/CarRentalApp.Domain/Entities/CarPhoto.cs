using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Domain.Entities
{
    public class CarPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } 

        // Foreign key for Car
        public int CarId { get; set; }
        // Navigation property for Car
        public Car? Car { get; set; }
    }
}
