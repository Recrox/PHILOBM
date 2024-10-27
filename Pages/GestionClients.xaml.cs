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
using System.Windows.Navigation;


namespace PHILOBM.Pages;

public partial class GestionClients : Page
{
    private readonly IClientService _clientService;
    public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

    public GestionClients()
    {
        
        InitializeComponent();
        _clientService = ServiceLocator.GetService<IClientService>();
    }

    private async void GestionClients_Loaded(object sender, RoutedEventArgs e)
    {
        await RefreshClientsAsync(); // Charger les clients à chaque fois que la page est chargée
    }

    private async Task RefreshClientsAsync()
    {
        Clients.Clear();
        var clients = await _clientService.GetAllAsync();
        ClientsListView.ItemsSource = clients;
        foreach (var client in clients)
        {
            Clients.Add(client);
        }
    }

    private void Rechercher_Click(object sender, RoutedEventArgs e)
    {
        string searchText = RechercheTextBox.Text.ToLower().Trim();

        // Filtrer les clients à l'aide de LINQ
        var clientFiltered = Clients
            .Where(client => client.LastName.ToLower().Contains(searchText) ||
                             client.FirstName.ToLower().Contains(searchText) ||
                             client.Phone.Contains(searchText) ||
                             client.Email.ToLower().Contains(searchText))
            .ToList(); // Convertir le résultat en liste

        // Mettre à jour l'affichage avec les clients filtrés
        ClientsListView.ItemsSource = clientFiltered;
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
