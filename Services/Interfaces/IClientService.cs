using PHILOBM.Models;
namespace PHILOBM.Services.Interfaces;

public interface IClientService : IBaseContextService<Client>
{
    Task<Client?> GetClientByIdWithCarsAsync(int clientId);
}
