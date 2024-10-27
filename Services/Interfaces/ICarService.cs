using PHILOBM.Models;
namespace PHILOBM.Services.Interfaces;

public interface ICarService : IBaseContextService<Car>
{
    Task<List<Car>> GetAllCarsByClientIdAsync(int clientId);
}