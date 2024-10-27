using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Models;
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

        var clientService = ServiceLocator.GetService<IClientService>();
        _clientService = clientService;
        // Récupérer le service à partir du conteneur DI
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
                await _clientService.DeleteAsync(clientASupprimer.Id);
                await this.RefreshClientsAsync();
                // Optionnel : Supprimez le client de la base de données ici si nécessaire
            }
        }
    }

    private async void AjouterClient_Click(object sender, RoutedEventArgs e)
    {
        // Vérifiez si les champs du client sont valides
        if (!IsClientValid())
        {
            //MessageBox.Show("Veuillez vérifier les informations saisies.\nLe nom doit contenir au moins 3 lettres et le numéro de téléphone doit être valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            MessageBox.Show("Veuillez remplir au moins un champ.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Créer une nouvelle instance de Client à partir des champs de texte
        var client = new Client
        {
            Nom = NomTextBox.Text,
            Prenom = PrenomTextBox.Text,
            Telephone = TelephoneTextBox.Text,
            Email = EmailTextBox.Text,
            Adresse = AdresseTextBox.Text,
        };

        await _clientService.AddAsync(client);
        await this.RefreshClientsAsync();
        ClearTextBox();
    }

    private bool IsClientValid()
    {
        //if (!string.IsNullOrWhiteSpace(NomTextBox.Text) && NomTextBox.Text.Length < 3)
        //    return false;

        //if (!IsPhoneNumberValid(TelephoneTextBox.Text))
        //    return false;

        return IsAnyFieldValid();
    }

    private bool IsPhoneNumberValid(string phoneNumber)
    {
        return !string.IsNullOrWhiteSpace(phoneNumber) &&
               phoneNumber.All(char.IsDigit) &&
               phoneNumber.Length >= 7; // Exemple de longueur minimale pour un numéro de téléphone
    }

    private bool IsAnyFieldValid()
    {
        return !string.IsNullOrWhiteSpace(PrenomTextBox.Text) ||
                !string.IsNullOrWhiteSpace(NomTextBox.Text) ||
               !string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
               !string.IsNullOrWhiteSpace(TelephoneTextBox.Text) ||
               !string.IsNullOrWhiteSpace(AdresseTextBox.Text);
    }


    private void ClientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ClientsListView.SelectedItem is Client selectedClient)
        {
            // Navigate to the details page and pass the selected client
            var detailsPage = new ClientDetails(selectedClient);
            NavigationService.Navigate(detailsPage);
            // Réinitialiser la sélection pour permettre un nouveau clic
            ClientsListView.SelectedItem = null;
        }
    }

    private void ClearTextBox()
    {
        NomTextBox.Clear();
        PrenomTextBox.Clear();
        TelephoneTextBox.Clear();
        EmailTextBox.Clear();
        AdresseTextBox.Clear();
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // Appeler la méthode d'ajout de client
            this.AjouterClient_Click(sender, e);
            e.Handled = true; // Empêche le son de "ding" lorsque Enter est pressé
        }
    }
}
