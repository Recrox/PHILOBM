using Microsoft.EntityFrameworkCore;
using PHILOBM.Database;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;

namespace PHILOBM.Services;

public class InvoiceService : BaseContextService<Invoice>, IInvoiceService
{
    public InvoiceService(PhiloBMContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Invoice>> GetInvoicesForClientAsync(int selectedClientId)
    {
        var invoices = await _context.Invoices
            .Include(invoice => invoice.Client) // Inclut les détails du client si nécessaire
            .Where(invoice => invoice.Client.Id == selectedClientId) // Assurez-vous que Client est correctement référencé
            .ToListAsync(); // Exécute la requête et retourne la liste des factures

        return invoices;
    }
}
