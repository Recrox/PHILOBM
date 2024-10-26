using PHILOBM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class GestionClients : Page
{
    public ObservableCollection<Client> Clients { get; set; }

    public GestionClients()
    {
        InitializeComponent();
        Clients = new ObservableCollection<Client>(); // Remplissez cette collection avec vos données
        ClientsListView.ItemsSource = Clients;
    }

    private void Rechercher_Click(object sender, RoutedEventArgs e)
    {
        string searchText = RechercheTextBox.Text.ToLower();
        var filteredClients = Clients.Where(c => c.Nom.ToLower().Contains(searchText) ||
                                                  c.Prenom.ToLower().Contains(searchText) ||
                                                  c.Telephone.Contains(searchText) ||
                                                  c.Email.ToLower().Contains(searchText)).ToList();

        // Mettre à jour l'affichage avec les clients filtrés
        ClientsListView.ItemsSource = filteredClients;
    }

    private void SupprimerClient_Click(object sender, RoutedEventArgs e)
    {
        Button boutonSupprimer = sender as Button;
        Client clientASupprimer = boutonSupprimer.Tag as Client;

        if (clientASupprimer != null)
        {
            MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {clientASupprimer.Nom} {clientASupprimer.Prenom} ?",
                "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Clients.Remove(clientASupprimer);
            }
        }
    }
    private void AjouterClient_Click(object sender, RoutedEventArgs e)
    {
        var client = new Client
        {
            Nom = NomTextBox.Text,
            Prenom = PrenomTextBox.Text,
            Telephone = TelephoneTextBox.Text,
            Email = EmailTextBox.Text
        };

        Clients.Add(client);
        ClientsListView.Items.Refresh();

        // Optionnel : Vider les champs après l'ajout
        NomTextBox.Clear();
        PrenomTextBox.Clear();
        TelephoneTextBox.Clear();
        EmailTextBox.Clear();
    }
}
