using PHILOBM.Models;
using System.Collections.Generic;
using System.Windows;

namespace PHILOBM;

public partial class GestionClients : Window
{
    private List<Client> clients;

    public GestionClients()
    {
        InitializeComponent();
        clients = new List<Client>();
        ClientsListView.ItemsSource = clients;
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

        clients.Add(client);
        ClientsListView.Items.Refresh();

        // Optionnel : Vider les champs après l'ajout
        NomTextBox.Clear();
        PrenomTextBox.Clear();
        TelephoneTextBox.Clear();
        EmailTextBox.Clear();
    }
}
