using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class InvoicePage : Page
{
    private readonly IInvoiceService _invoiceService;
    private readonly int _selectedClientId; // Stocke l'ID du client sélectionné
    public InvoicePage(int selectedClientId)
    {
        InitializeComponent();
        _invoiceService = ServiceLocator.GetService<IInvoiceService>();
        _selectedClientId = selectedClientId;
        DataContext = this; // Assurez-vous de définir le DataContext pour lier les données
        LoadInvoices();
    }

    public ObservableCollection<Invoice> Invoices { get; set; } = new ObservableCollection<Invoice>();

    private async void LoadInvoices()
    {
        // Chargez l'historique des factures pour le client sélectionné
        // Cela pourrait venir d'un service ou d'une base de données
        var invoices = await _invoiceService.GetInvoicesForClientAsync(_selectedClientId);
        foreach (var invoice in invoices)
        {
            Invoices.Add(invoice);
        }
    }

    private void PrintInvoice(Invoice invoice)
    {
        invoice.CreerPDF(); // Appelle la méthode pour créer le PDF
        // Ajouter une logique pour ouvrir le fichier PDF ou l'envoyer à l'imprimante
    }
}
