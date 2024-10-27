using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace PHILOBM.Pages.Details;

public partial class ClientDetails : Page
{
    private readonly IClientService _clientService;
    private readonly ICarService _carService;
    private Client _client;

    private string _buttonContent;
    public string ButtonContent
    {
        get => _buttonContent;
        set
        {
            _buttonContent = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void ClientDetails_Loaded(object sender, RoutedEventArgs e)
    {
        // Vérifier si _client est null avant d'appeler la méthode
        if (_client == null)
        {
            return;
        }
        await RefreshClientDetails(_client.Id); // Charger les détails du client
    }


    public ClientDetails(int? clientId = null)
    {
        InitializeComponent();
        _clientService = ServiceLocator.GetService<IClientService>();
        _carService = ServiceLocator.GetService<ICarService>();

        // Définir le contenu du bouton en fonction de la présence du clientId
        ButtonContent = clientId == null ? "Ajouter le client" : "Mettre à jour le client";

        if (clientId != null)
            _= RefreshClientDetails(clientId.Value);

        DataContext = this; // Assurez-vous que le DataContext est bien défini

        // Désactiver le bouton de mise à jour au démarrage
        UpdateOrAddClientButton.IsEnabled = false; 
    }

    private async Task RefreshClientDetails(int clientId)
    {
        _client = await _clientService.GetClientByIdWithCarsAsync(clientId) ?? throw new Exception("Client don't exist");

        NameTextBox.Text = _client.LastName;
        FirstNameTextBox.Text = _client.FirstName;
        PhoneTextBox.Text = _client.Phone;
        EmailTextBox.Text = _client.Email;
        AddressTextBox.Text = _client.Address;

        var cars = await _carService.GetAllCarsByClientIdAsync(_client.Id);
        _client.Cars = new ObservableCollection<Car>(cars);
        CarsListView.ItemsSource = _client.Cars; // Utilisez ItemsSource pour lier la collection de voitures
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateOrAddClientButton.IsEnabled = true; // Activez le bouton de mise à jour
    }

    private async void UpdateOrAddClient_Click(object sender, RoutedEventArgs e)
    {
        if (_client is null)
        {
            // Créer une nouvelle instance de Client à partir des champs de texte
            var client = new Client
            {
                LastName = NameTextBox.Text,
                FirstName = FirstNameTextBox.Text,
                Phone = PhoneTextBox.Text,
                Email = EmailTextBox.Text,
                Address = AddressTextBox.Text,
            };

            await _clientService.AddAsync(client);
            Retour_Click(sender, e);
            return;
        }
        // Update client details from the text boxes
        _client.LastName = NameTextBox.Text;
        _client.FirstName = FirstNameTextBox.Text;
        _client.Phone = PhoneTextBox.Text;
        _client.Email = EmailTextBox.Text;
        _client.Address = AddressTextBox.Text;

        await _clientService.UpdateAsync(_client); // Ensure this method exists
        //MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        UpdateOrAddClientButton.IsEnabled = false; // Désactivez à nouveau le bouton
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
        NavigationService.Navigate(new CarDetails(clientId:_client.Id));
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
                await RefreshClientDetails(_client.Id);
            }
        }
    }

    private void BillingButton_Click(object sender, RoutedEventArgs e)
    {
        var invoicePage = new InvoicePage(_client.Id);
        NavigationService.Navigate(invoicePage);
    }

    private void CarsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CarsListView.SelectedItem is Car selectedCar)
        {
            // Vérifiez si une voiture a été sélectionnée
            if (selectedCar != null)
            {
                // Naviguez vers la page de détails de la voiture
                var carDetailPage = new CarDetails(carId: selectedCar.Id, clientId: _client.Id); // Passez l'objet `Car` à votre page de détails
                NavigationService?.Navigate(carDetailPage); // Utilisez NavigationService pour naviguer
                CarsListView.SelectedItem = null;
            }
        }
    }
}
