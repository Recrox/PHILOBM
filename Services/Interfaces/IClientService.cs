using PHILOBM.Models;
namespace PHILOBM.Services.Interfaces;

public interface IClientService
{
    Task AjouterClient(Client client);
    Task<List<Client>> ChargerClients();

    Task MettreAJourClient(Client client);
    Task SupprimerClient(int id);
}
