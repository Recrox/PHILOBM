using System.Windows;
using System.Windows.Controls;
using PHILOBM.Models;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PHILOBM.Pages;

public partial class AddCar : Page
{
    private readonly ICarService _carService;
    private readonly IClientService _clientService;
    private Client _client;

    public AddCar(Client client)
    {
        InitializeComponent();
        _client = client;
        _clientService = ServiceLocator.GetService<IClientService>();
        _carService = ServiceLocator.GetService<ICarService>();
    }

    private async void AddCar_Click(object sender, RoutedEventArgs e)
    {
        // Récupérez les valeurs des champs
        string licensePlate = LicensePlateTextBox.Text;
        string chassisNumber = ChassisNumberTextBox.Text;

        if (int.TryParse(MileageTextBox.Text, out int mileage))
        {
            // Créez une nouvelle voiture et l'associez au client
            Car newCar = new Car
            {
                LicensePlate = licensePlate,
                ChassisNumber = chassisNumber,
                Mileage = mileage,
                ClientId = _client.Id, // Assurez-vous que l'ID du client est disponible
                Owner = _client // Optionnel si vous souhaitez aussi définir le propriétaire
            };

            // Ajoutez la voiture à la base de données
            await _carService.AddAsync(newCar);

            // Retour à la page de détails du client
            NavigationService.GoBack(); // ou utilisez Frame.Navigate si vous naviguez avec Frame
        }
        else
        {
            MessageBox.Show("Veuillez entrer un kilométrage valide.");
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack(); // Annule et revient à la page précédente
    }
}
