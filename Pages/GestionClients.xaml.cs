using PHILOBM.Models;
using PHILOBM.Pages.Details;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.ComponentModel;

namespace PHILOBM.Pages;

public partial class GestionClients : Page
{
    private readonly IClientService _clientService;
    public ObservableCollection<Client> Clients { get; set; }
    private ICollectionView _clientsView;

    public GestionClients()
    {
        InitializeComponent();
        Clients = new ObservableCollection<Client>();
        _clientService = ServiceLocator.GetService<IClientService>();
        _clientsView = CollectionViewSource.GetDefaultView(Clients);
        ClientsListView.ItemsSource = _clientsView;
        _ = RefreshClientsAsync(); // Charger les clients lors de l'initialisation
    }

    private async Task RefreshClientsAsync()
    {
        Clients.Clear();
        var clients = await _clientService.GetAllAsync();
        foreach (var client in clients)
        {
            Clients.Add(client);
        }
    }

    private void Rechercher_Click(object sender, RoutedEventArgs e)
    {
        string searchText = RechercheTextBox.Text.ToLower();
        _clientsView.Filter = client =>
        {
            if (client is Client c)
            {
                return c.LastName.ToLower().Contains(searchText) ||
                       c.FirstName.ToLower().Contains(searchText) ||
                       c.Phone.Contains(searchText) ||
                       c.Email.ToLower().Contains(searchText);
            }
            return false;
        };
    }

    private async void SupprimerClient_ClickAsync(object sender, RoutedEventArgs e)
    {
        Button boutonSupprimer = sender as Button;
        Client clientASupprimer = boutonSupprimer.Tag as Client;

        if (clientASupprimer != null)
        {
            MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {clientASupprimer.LastName} {clientASupprimer.FirstName} ?",
                "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await _clientService.DeleteAsync(clientASupprimer.Id);
                Clients.Remove(clientASupprimer); // Met à jour l'ObservableCollection
            }
        }
    }

    private void ClientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientsListView.SelectedItem is Client selectedClient)
        {
            var detailsPage = new ClientDetails(selectedClient.Id);
            NavigationService.Navigate(detailsPage);
            ClientsListView.SelectedItem = null; // Réinitialiser la sélection
        }
    }

    private void AjouterClient_Click(object sender, EventArgs e)
    {
        var detailsPage = new ClientDetails();
        NavigationService.Navigate(detailsPage);
    }
}
