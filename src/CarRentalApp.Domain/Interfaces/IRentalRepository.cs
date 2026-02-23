using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<Rental?> GetRentalWithDetailsAsync(int rentalId);
        Task<IEnumerable<Rental>> GetRentalsByClientIdAsync(int clientId);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId);
    }
}
