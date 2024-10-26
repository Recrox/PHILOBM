using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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

        var clientService = App.AppHost.Services.GetRequiredService<IClientService>();
        _clientService = clientService;
        // Récupérer le service à partir du conteneur DI
        _ = RefreshClientsAsync(); // Charger les clients lors de l'initialisation
    }

    private async Task RefreshClientsAsync()
    {
        Clients.Clear();
        var clients = await _clientService.GetAll();
        foreach (var client in clients)
        {
            Clients.Add(client);
        }
    }

    private void Rechercher_Click(object sender, RoutedEventArgs e)
    {
        string searchText = RechercheTextBox.Text.ToLower();
        var filteredClients = Clients.Where(c => c.Nom.ToLower().Contains(searchText) ||
                                                  c.Prenom.ToLower().Contains(searchText) ||
                                                  c.Telephone.Contains(searchText) ||
                                                  c.Email.ToLower().Contains(searchText)).ToList();

        ClientsListView.ItemsSource = filteredClients;
    }

    private async void SupprimerClient_ClickAsync(object sender, RoutedEventArgs e)
    {
        Button boutonSupprimer = sender as Button;
        Client clientASupprimer = boutonSupprimer.Tag as Client;

        if (clientASupprimer != null)
        {
            MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {clientASupprimer.Nom} {clientASupprimer.Prenom} ?",
                "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await _clientService.Delete(clientASupprimer.Id);
                await this.RefreshClientsAsync();
                // Optionnel : Supprimez le client de la base de données ici si nécessaire
            }
        }
    }

    private async void AjouterClient_Click(object sender, RoutedEventArgs e)
    {
        // Créer une nouvelle instance de Client à partir des champs de texte
        var client = new Client
        {
            Nom = NomTextBox.Text,
            Prenom = PrenomTextBox.Text,
            Telephone = TelephoneTextBox.Text,
            Email = EmailTextBox.Text
        };

        await _clientService.Add(client);
        await this.RefreshClientsAsync();
        ClearTextBox();
    }

    private void ClientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientsListView.SelectedItem is Client selectedClient)
        {
            // Navigate to the details page and pass the selected client
            var detailsPage = new ClientDetails(selectedClient);
            NavigationService.Navigate(detailsPage);
        }
    }

    private void ClearTextBox()
    {
        NomTextBox.Clear();
        PrenomTextBox.Clear();
        TelephoneTextBox.Clear();
        EmailTextBox.Clear();
    }
}
