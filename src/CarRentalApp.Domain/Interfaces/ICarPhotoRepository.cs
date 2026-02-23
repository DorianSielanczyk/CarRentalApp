using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalApp.Domain.Entities;

namespace CarRentalApp.Domain.Interfaces
{
    public interface ICarPhotoRepository : IRepository<CarPhoto>
    {
        Task<IEnumerable<CarPhoto>> GetPhotosByCarIdAsync(int carId);
        Task<CarPhoto?> GetMainPhotoByCarIdAsync(int carId);
    }
}
