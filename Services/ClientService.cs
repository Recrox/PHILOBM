using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;
namespace PHILOBM.Services;
public class ClientService : BaseContextService<Client>, IClientService
{
    public ClientService(PhiloBMContext context) : base(context)
    {
    }

    public async Task<Client?> GetClientByIdWithCarsAsync(int clientId)
    {
        return await _context.Clients
            .Include(c => c.Cars) // Inclut les voitures associées
            .FirstOrDefaultAsync(c => c.Id == clientId);
    }
}
