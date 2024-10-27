using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;

namespace PHILOBM.Services;

public class CarService : BaseContextService<Car>, ICarService
{
    public CarService(PhiloBMContext context) : base(context)
    {

    }

    public async Task<List<Car>> GetAllCarsByClientIdAsync(int clientId)
    {
        return await _context.Cars.Where(car => car.ClientId == clientId).ToListAsync();
    }
}
