using PHILOBM.Models;
using PHILOBM.Models.Base;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class InvoicePage : Page
{
    private readonly IInvoiceService _invoiceService;
    private readonly int _selectedClientId; // Stocke l'ID du client sélectionné
    public ObservableCollection<Invoice> Invoices { get; set; } = new ObservableCollection<Invoice>();
    public InvoicePage(int selectedClientId)
    {
        InitializeComponent();
        _invoiceService = ServiceLocator.GetService<IInvoiceService>();
        _selectedClientId = selectedClientId;
        DataContext = this; // Assurez-vous de définir le DataContext pour lier les données
        //LoadInvoices();
        LoadMockInvoices();
    }

    

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

    private void PrintInvoice_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var invoice = button.CommandParameter as Invoice; // Récupère l'objet Invoice
            if (invoice != null)
            {
                _invoiceService.CreerPDF(invoice); // Crée le PDF

                // Afficher une fenêtre contextuelle de confirmation
                MessageBox.Show("La facture a été imprimée avec succès.", "Impression Terminée", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Erreur : Facture non trouvée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }



    private void LoadMockInvoices()
    {
        // Créez une liste de factures fictives
        Invoices.Add(new Invoice
        {
            Id = 1,
            Client = new Client { Id = 1, LastName = "Dupont", FirstName = "Arno"},
            Car = new Car { LicensePlate = "ABC 123" },
            Date = DateTime.Now.AddDays(-10),
            Services = new List<Service>
                {
                    new Service { Description = "Changement d'huile", Price = 50 },
                    new Service { Description = "Réparation de frein", Price = 120 }
                }
        });

        Invoices.Add(new Invoice
        {
            Id = 2,
            Client = new Client { Id = 2, LastName = "Martin", FirstName = "Arno" },
            Car = new Car { LicensePlate = "XYZ 789" },
            Date = DateTime.Now.AddDays(-5),
            Services = new List<Service>
                {
                    new Service { Description = "Contrôle technique", Price = 80 },
                    new Service { Description = "Remplacement de pneus", Price = 300 }
                }
        });

        Invoices.Add(new Invoice
        {
            Id = 3,
            Client = new Client { Id = 3, LastName = "Bernard", FirstName = "Arno" },
            Car = new Car { LicensePlate = "LMN 456" },
            Date = DateTime.Now.AddDays(-2),
            Services = new List<Service>
                {
                    new Service { Description = "Révision complète", Price = 150 },
                    new Service { Description = "Changement de batterie", Price = 120 }
                }
        });
    }
}
