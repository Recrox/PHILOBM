using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;
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

    public async Task MettreAJourClient(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task SupprimerClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

}
