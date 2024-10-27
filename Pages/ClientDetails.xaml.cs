using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class ClientDetails : Page
{
    private readonly IClientService _clientService;
    private readonly ICarService _carService;
    private Client _client;
    

    public ClientDetails(int clientId)
    {
        InitializeComponent();
        _clientService = ServiceLocator.GetService<IClientService>();
        _carService = ServiceLocator.GetService<ICarService>();


        // Load client data into text boxes
        RefreshClientDetails(clientId);

        // Désactiver le bouton de mise à jour au démarrage
        UpdateClientButton.IsEnabled = false; // Assurez-vous que le bouton a ce nom
    }

    private async void RefreshClientDetails(int clientId)
    {
        _client = await _clientService.GetClientByIdWithCarsAsync(clientId) ?? throw new Exception("Client don't exist");

        NameTextBox.Text = _client.LastName;
        FirstNameTextBox.Text = _client.FirstName;
        PhoneTextBox.Text = _client.Phone;
        EmailTextBox.Text = _client.Email;

        // Utiliser le ListView pour afficher les voitures
        CarsListView.ItemsSource = _client.Cars; // Utilisez ItemsSource pour lier la collection de voitures


        // Ajoutez des gestionnaires d'événements pour détecter les modifications
        NameTextBox.TextChanged += TextBox_TextChanged;
        FirstNameTextBox.TextChanged += TextBox_TextChanged;
        PhoneTextBox.TextChanged += TextBox_TextChanged;
        EmailTextBox.TextChanged += TextBox_TextChanged;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateClientButton.IsEnabled = true; // Activez le bouton de mise à jour
    }

    private async void UpdateClient_Click(object sender, RoutedEventArgs e)
    {
        // Update client details from the text boxes
        _client.LastName = NameTextBox.Text;
        _client.FirstName = FirstNameTextBox.Text;
        _client.Phone = PhoneTextBox.Text;
        _client.Email = EmailTextBox.Text;

        await _clientService.UpdateAsync(_client); // Ensure this method exists
        //MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
        //CarsListView.SelectedItem = null;
    }

    private async void DeleteCarButton_Click(object sender, RoutedEventArgs e)
    {
        Button deleteButton = sender as Button;
        Car carToDelete = deleteButton.Tag as Car;

        if (carToDelete != null)
        {
            MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la voiture {carToDelete.Brand} {carToDelete.Model} ?",
                "Confirmation de suppression", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await _carService.DeleteAsync(carToDelete.Id); // Suppression de la voiture
                RefreshClientDetails(_client.Id);
            }
        }
    }
}
