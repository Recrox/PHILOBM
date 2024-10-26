using PHILOBM.Models;
namespace PHILOBM.Services;

public interface IClientService
{
    Task AjouterClient(Client client);
    Task<List<Client>> ChargerClients();
}
