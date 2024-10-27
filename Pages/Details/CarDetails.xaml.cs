using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages.Details;

public partial class CarDetails : Page
{
    private readonly ICarService _carService;
    private readonly IClientService _clientService;
    private Car _car;
    private int? _currentClientId;


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

    public CarDetails(int? carId = null, int? clientId = null)
    {
        InitializeComponent();
        _carService = ServiceLocator.GetService<ICarService>();
        _clientService = ServiceLocator.GetService<IClientService>();

        // Définir le contenu du bouton en fonction de la présence du clientId
        ButtonContent = carId == null ? "Ajouter la voiture" : "Mettre à jour la voiture";

        _currentClientId = clientId;

        if (carId != null)
            _ = RefreshCarDetails(carId.Value);

        _=LoadClients();

        

        DataContext = this; // Assurez-vous que le DataContext est bien défini

    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateOrAddCarButton.IsEnabled = true; // Activez le bouton de mise à jour
    }

    private async Task RefreshCarDetails(int carId)
    {
        _car = await _carService.GetCarByIdWithServicesAsync(carId) ?? throw new Exception("Car doesn't exist");

        // Remplir les TextBox avec les détails de la voiture
        BrandTextBox.Text = _car.Brand;
        ModelTextBox.Text = _car.Model;
        LicensePlateTextBox.Text = _car.LicensePlate;
        ChassisNumberTextBox.Text = _car.ChassisNumber;
        MileageTextBox.Text = _car.Mileage.ToString();


        // Utiliser le ListView pour afficher les services
        ServicesListView.ItemsSource = _car.Services; // Utilisez ItemsSource pour lier la collection de services
    }

    private async void UpdateOrAddCar_Click(object sender, RoutedEventArgs e)
    {
        int? selectedClientId = (int?)ClientComboBox.SelectedValue; // Récupérer l'ID du client sélectionné
        if (_car is null)
        {

            // Créer une nouvelle instance de Car à partir des champs de texte
            var car = new Car
            {
                LicensePlate = LicensePlateTextBox.Text,
                ChassisNumber = ChassisNumberTextBox.Text,
                Mileage = int.TryParse(MileageTextBox.Text, out int mileage) ? mileage : 0,
                Brand = BrandTextBox.Text,
                Model = ModelTextBox.Text,
                ClientId = _currentClientId,
            };

            await _carService.AddAsync(car); // Assurez-vous que cette méthode existe
            Retour_Click(sender, e); // Retourne à la page précédente
            return;
        }

        // Mettre à jour les détails de la voiture à partir des champs de texte
        _car.LicensePlate = LicensePlateTextBox.Text;
        _car.ChassisNumber = ChassisNumberTextBox.Text;
        _car.Mileage = int.TryParse(MileageTextBox.Text, out int updatedMileage) ? updatedMileage : _car.Mileage; // Conserve l'ancienne valeur si le parsing échoue
        _car.Brand = BrandTextBox.Text;
        _car.Model = ModelTextBox.Text;
        _car.ClientId = _currentClientId; // Assigner l'ID du client lors de la mise à jour

        await _carService.UpdateAsync(_car); // Assurez-vous que cette méthode existe

        // MessageBox.Show("Car updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        UpdateOrAddCarButton.IsEnabled = false; // Désactiver le bouton
    }

    private async Task LoadClients()
    {
        var clients = await _clientService.GetAllAsync(); // Assurez-vous d'avoir ce service
        ClientComboBox.ItemsSource = clients;
    }


    private void Retour_Click(object sender, RoutedEventArgs e)
    {
        // Naviguer vers la page précédente
        if (NavigationService.CanGoBack)
        {
            NavigationService.GoBack();
        }
    }
}
