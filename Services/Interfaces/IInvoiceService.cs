using PHILOBM.Models;

namespace PHILOBM.Services.Interfaces;

public interface IInvoiceService : IBaseContextService<Invoice>
{
    Task<IEnumerable<Invoice>> GetInvoicesForClientAsync(int selectedClientId);
}