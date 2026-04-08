using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface IRentalPhotoRepository : IRepository<RentalPhoto>
    {
        Task<IEnumerable<RentalPhoto>> GetPhotosByRentalIdAsync(int rentalId);
    }
}