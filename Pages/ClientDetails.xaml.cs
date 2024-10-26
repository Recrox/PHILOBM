using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Models;
using PHILOBM.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class ClientDetails : Page
{
    private readonly IClientService _clientService;
    private Client _client;

    public ClientDetails(Client client)
    {
        InitializeComponent();
        _clientService = App.AppHost?.Services.GetRequiredService<IClientService>() ?? throw new Exception("IClientService not loaded");
        _client = client;

        // Load client data into text boxes
        LoadClientDetails();
    }

    private void LoadClientDetails()
    {
        NameTextBox.Text = _client.Nom;
        FirstNameTextBox.Text = _client.Prenom;
        PhoneTextBox.Text = _client.Telephone;
        EmailTextBox.Text = _client.Email;
    }

    private async void UpdateClient_Click(object sender, RoutedEventArgs e)
    {
        // Update client details from the text boxes
        _client.Nom = NameTextBox.Text;
        _client.Prenom = FirstNameTextBox.Text;
        _client.Telephone = PhoneTextBox.Text;
        _client.Email = EmailTextBox.Text;

        await _clientService.Update(_client); // Ensure this method exists
        MessageBox.Show("Client updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        // Optionally, navigate back to the previous page
        // NavigationService.GoBack();
    }
}
