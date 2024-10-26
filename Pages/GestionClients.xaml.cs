using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Models;
using PHILOBM.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages
{
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
            ChargerClientsAsync(); // Charger les clients lors de l'initialisation
        }

        private async void ChargerClientsAsync()
        {
            var clients = await _clientService.ChargerClients();
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
                    // Optionnel : Supprimez le client de la base de données ici si nécessaire
                }
            }
        }

        private async void AjouterClient_Click(object sender, RoutedEventArgs e)
        {
            var client = new Client
            {
                Nom = NomTextBox.Text,
                Prenom = PrenomTextBox.Text,
                Telephone = TelephoneTextBox.Text,
                Email = EmailTextBox.Text
            };

            await _clientService.AjouterClient(client);
            Clients.Add(client); // Ajoutez le client à la collection locale
            NomTextBox.Clear();
            PrenomTextBox.Clear();
            TelephoneTextBox.Clear();
            EmailTextBox.Clear();
        }
    }
}
