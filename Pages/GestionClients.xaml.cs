using PHILOBM.Models;
using PHILOBM.Pages.Details;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PHILOBM.Pages;

public partial class GestionClients : Page
{
    private readonly IClientService _clientService;
    public ObservableCollection<Client> Clients { get; set; }

    public GestionClients()
    {
        InitializeComponent();
        Clients = new ObservableCollection<Client>();
        ClientsListView.ItemsSource = Clients;
        _clientService = ServiceLocator.GetService<IClientService>(); ;
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
        var filteredClients = Clients.Where(c => c.LastName.ToLower().Contains(searchText) ||
                                                  c.FirstName.ToLower().Contains(searchText) ||
                                                  c.Phone.Contains(searchText) ||
                                                  c.Email.ToLower().Contains(searchText)).ToList();

        ClientsListView.ItemsSource = filteredClients;
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
                await this.RefreshClientsAsync();
            }
        }
    }

    private void ClientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientsListView.SelectedItem is Client selectedClient)
        {
            // Navigate to the details page and pass the selected client
            var detailsPage = new ClientDetails(selectedClient.Id);
            NavigationService.Navigate(detailsPage);
            // Réinitialiser la sélection pour permettre un nouveau clic
            ClientsListView.SelectedItem = null;
        }
    }
    
    private void AjouterClient_Click(object sender, EventArgs e)
    {
        // Navigate to the details page and pass the selected client
        var detailsPage = new ClientDetails();
        NavigationService.Navigate(detailsPage);
        // Réinitialiser la sélection pour permettre un nouveau clic
    }

    
}
