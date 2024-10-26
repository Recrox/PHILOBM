using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models;
namespace PHILOBM.Services;
public class ClientService : IClientService
{
    private readonly PhiloBMContext _context;

    public ClientService(PhiloBMContext context)
    {
        _context = context;
    }

    public async Task AjouterClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Client>> ChargerClients()
    {
        return await _context.Clients.ToListAsync();
    }

}
