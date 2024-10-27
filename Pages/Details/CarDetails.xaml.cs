using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages.Details;

public partial class CarDetails : Page
{
    private readonly ICarService _carService;
    private Car _car;

    public CarDetails(int carId)
    {
        InitializeComponent();
        _carService = ServiceLocator.GetService<ICarService>();

        // Charger les données de la voiture
        RefreshCarDetails(carId);
    }

    private async void RefreshCarDetails(int carId)
    {
        _car = await _carService.GetCarByIdWithServicesAsync(carId) ?? throw new Exception("Car doesn't exist");

        // Remplir les TextBox avec les détails de la voiture
        BrandTextBox.Text = _car.Brand;
        ModelTextBox.Text = _car.Model;
        LicensePlateTextBox.Text = _car.LicensePlate;
        ChassisNumberTextBox.Text = _car.ChassisNumber;

        // Utiliser le ListView pour afficher les services
        ServicesListView.ItemsSource = _car.Services; // Utilisez ItemsSource pour lier la collection de services
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
