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
        // Créez une liste de factures fictives avec des adresses pour chaque client
        Invoices.Add(new Invoice
        {
            Id = 1,
            Client = new Client
            {
                Id = 1,
                LastName = "Dupont",
                FirstName = "Arno",
                Address = "12 Rue de la Paix, 75002 Paris, France"
            },
            Car = new Car { LicensePlate = "ABC 123" },
            Date = DateTime.Now.AddDays(-10),
            Services = new List<Service>
        {
            new Service { Description = "Changement d'huile", Price = 50, Units = 1 },
            new Service { Description = "Réparation de frein", Price = 120, Units = 2 },
            new Service { Description = "Vérification des niveaux", Price = 30, Units = 1 },
            new Service { Description = "Changement de filtre à air", Price = 40, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 }
        }
        });

        Invoices.Add(new Invoice
        {
            Id = 2,
            Client = new Client
            {
                Id = 2,
                LastName = "Martin",
                FirstName = "Julie",
                Address = "48 Avenue des Champs-Élysées, 75008 Paris, France"
            },
            Car = new Car { LicensePlate = "XYZ 789" },
            Date = DateTime.Now.AddDays(-5),
            Services = new List<Service>
        {
            new Service { Description = "Contrôle technique", Price = 80, Units = 1 },
            new Service { Description = "Remplacement de pneus", Price = 300, Units = 4 },
            new Service { Description = "Réparation de la climatisation", Price = 150, Units = 1 },
            new Service { Description = "Changement de plaquettes de frein", Price = 90, Units = 2 },
            new Service { Description = "Vérification des freins", Price = 50, Units = 1 }
        }
        });

        Invoices.Add(new Invoice
        {
            Id = 3,
            Client = new Client
            {
                Id = 3,
                LastName = "Bernard",
                FirstName = "Luc",
                Address = "15 Place de l'Hôtel de Ville, 69001 Lyon, France"
            },
            Car = new Car { LicensePlate = "LMN 456" },
            Date = DateTime.Now.AddDays(-2),
            Services = new List<Service>
        {
            new Service { Description = "Révision complète", Price = 150, Units = 1 },
            new Service { Description = "Changement de batterie", Price = 120, Units = 1 },
            new Service { Description = "Nettoyage intérieur et extérieur", Price = 80, Units = 1 },
            new Service { Description = "Diagnostic électronique", Price = 60, Units = 1 },
            new Service { Description = "Changement de liquide de refroidissement", Price = 50, Units = 1 }
        }
        });

        // Ajout d'une facture avec un grand nombre de services
        Invoices.Add(new Invoice
        {
            Id = 4,
            Client = new Client
            {
                Id = 4,
                LastName = "Dubois",
                FirstName = "Claire",
                Address = "25 Rue de Rivoli, 75001 Paris, France"
            },
            Car = new Car { LicensePlate = "JKL 890" },
            Date = DateTime.Now.AddDays(-1),
            Services = new List<Service>
        {
            new Service { Description = "Changement d'huile", Price = 50, Units = 1 },
            new Service { Description = "Réparation de frein", Price = 120, Units = 2 },
            new Service { Description = "Vérification des niveaux", Price = 30, Units = 1 },
            new Service { Description = "Changement de filtre à air", Price = 40, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 },
            new Service { Description = "Remplacement des balais d'essuie-glace", Price = 20, Units = 2 },
            new Service { Description = "Vérification de la batterie", Price = 25, Units = 1 },
            new Service { Description = "Contrôle de l'éclairage", Price = 15, Units = 1 },
            new Service { Description = "Remplacement de courroie", Price = 200, Units = 1 },
            new Service { Description = "Réglage des phares", Price = 40, Units = 1 },
            new Service { Description = "Changement de liquide de frein", Price = 60, Units = 1 },
            new Service { Description = "Service de climatisation", Price = 100, Units = 1 },
            new Service { Description = "Mise à jour du logiciel du véhicule", Price = 75, Units = 1 },
            new Service { Description = "Inspection des freins", Price = 50, Units = 1 },
            new Service { Description = "Changement de bougies d'allumage", Price = 80, Units = 4 },
            new Service { Description = "Changement de filtre à huile", Price = 30, Units = 1 },
            new Service { Description = "Diagnostic de performance", Price = 90, Units = 1 },
            new Service { Description = "Vérification des niveaux de liquide", Price = 20, Units = 1 },
            new Service { Description = "Contrôle des pneus", Price = 50, Units = 1 },
            new Service { Description = "Inspection de sécurité", Price = 100, Units = 1 },
            new Service { Description = "Vérification des amortisseurs", Price = 60, Units = 1 },
            new Service { Description = "Changement de filtre à carburant", Price = 45, Units = 1 },
            new Service { Description = "Réparation du système d'échappement", Price = 120, Units = 1 },
            new Service { Description = "Contrôle de l'ABS", Price = 70, Units = 1 },
            new Service { Description = "Vérification de la direction", Price = 50, Units = 1 },
            new Service { Description = "Changement de pneus", Price = 80, Units = 4 },
            new Service { Description = "Réparation de la climatisation", Price = 150, Units = 1 },
            new Service { Description = "Révision complète du moteur", Price = 300, Units = 1 },
            new Service { Description = "Changement de liquide de refroidissement", Price = 50, Units = 1 },
            new Service { Description = "Contrôle de l'état des freins", Price = 40, Units = 1 },
            new Service { Description = "Réparation de la transmission", Price = 200, Units = 1 },
            new Service { Description = "Réparation de la suspension", Price = 150, Units = 1 },
            new Service { Description = "Changement de filtre à particules", Price = 100, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 },
            new Service { Description = "Mise à jour du GPS", Price = 40, Units = 1 }
        }
        });
    }



}
