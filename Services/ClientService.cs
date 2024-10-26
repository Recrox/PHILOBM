using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;
namespace PHILOBM.Services;
public class ClientService : BaseContextService<Client>, IClientService
{
    public ClientService(PhiloBMContext context) : base(context)
    {
    }
}