using System.Reflection;
using System.Windows;

namespace PHILOBM;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Nouveau_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour créer un nouveau document
        MessageBox.Show("Créer un nouveau document.");
    }

    private void Ouvrir_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour ouvrir un document
        MessageBox.Show("Ouvrir un document existant.");
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour enregistrer le document
        MessageBox.Show("Enregistrer le document.");
    }

    private void Quitter_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour quitter l'application
        Application.Current.Shutdown();
    }

    private void Clients_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour gérer les clients
        MessageBox.Show("Gérer les clients.");
    }

    private void Voitures_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour gérer les voitures
        MessageBox.Show("Gérer les voitures.");
    }

    private void Factures_Click(object sender, RoutedEventArgs e)
    {
        // Logique pour gérer les factures
        MessageBox.Show("Gérer les factures.");
    }

    private void Apropos_Click(object sender, RoutedEventArgs e)
    {
        var versionInfo = Assembly.GetExecutingAssembly().GetName().Version;
        string version = versionInfo?.ToString() ?? "undefined";
        MessageBox.Show($"PHILO B.M - Version {version}");
    }

    private void GestionClients_Click(object sender, RoutedEventArgs e)
    {
        GestionClients gestionClientsWindow = new GestionClients();
        gestionClientsWindow.Show();
    }


}