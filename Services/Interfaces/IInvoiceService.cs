﻿using PHILOBM.Models;

namespace PHILOBM.Services.Interfaces;

public interface IInvoiceService : IBaseContextService<Invoice>
{
    void CreerPDF(Invoice invoice);
    void CreerExcel(Invoice invoice);
    Task<IEnumerable<Invoice>> GetInvoicesForClientAsync(int selectedClientId);
}