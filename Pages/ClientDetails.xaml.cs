using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class ClientDetails : Page
{
    private readonly IClientService _clientService;
    private Client _client;
    private bool _isModified = false; // Ajout de la variable pour suivre les modifications

    public ClientDetails(Client client)
    {
        InitializeComponent();
        _clientService = ServiceLocator.GetService<IClientService>();
        _client = client;

        // Load client data into text boxes
        LoadClientDetails();

        // Désactiver le bouton de mise à jour au démarrage
        UpdateClientButton.IsEnabled = false; // Assurez-vous que le bouton a ce nom
    }

    private void LoadClientDetails()
    {
        NameTextBox.Text = _client.Nom;
        FirstNameTextBox.Text = _client.Prenom;
        PhoneTextBox.Text = _client.Telephone;
        EmailTextBox.Text = _client.Email;

        // Utiliser le ListView pour afficher les voitures
        CarsListView.ItemsSource = _client.Voitures; // Utilisez ItemsSource pour lier la collection de voitures


        // Ajoutez des gestionnaires d'événements pour détecter les modifications
        NameTextBox.TextChanged += TextBox_TextChanged;
        FirstNameTextBox.TextChanged += TextBox_TextChanged;
        PhoneTextBox.TextChanged += TextBox_TextChanged;
        EmailTextBox.TextChanged += TextBox_TextChanged;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Vérifiez si les données ont changé
        _isModified = true;
        UpdateClientButton.IsEnabled = true; // Activez le bouton de mise à jour
    }

    private async void UpdateClient_Click(object sender, RoutedEventArgs e)
    {
        // Update client details from the text boxes
        _client.Nom = NameTextBox.Text;
        _client.Prenom = FirstNameTextBox.Text;
        _client.Telephone = PhoneTextBox.Text;
        _client.Email = EmailTextBox.Text;

        await _clientService.UpdateAsync(_client); // Ensure this method exists
        //MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        _isModified = false;
        UpdateClientButton.IsEnabled = false; // Désactivez à nouveau le bouton
    }

    private void Retour_Click(object sender, RoutedEventArgs e)
    {
        // Naviguer vers la page précédente
        if (NavigationService.CanGoBack)
        {
            NavigationService.GoBack();
        }
    }

    private void AddCarButton_Click(object sender, RoutedEventArgs e)
    {
        AddCar addCarPage = new AddCar(_client); // Passez le client actuel à la page AddCar
        NavigationService.Navigate(addCarPage);
    }
}
