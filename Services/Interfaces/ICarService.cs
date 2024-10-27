using PHILOBM.Models;
namespace PHILOBM.Services.Interfaces;

public interface ICarService : IBaseContextService<Car>
{
    Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId);
    Task<Car?> GetCarByIdWithServicesAsync(int carId);
}
