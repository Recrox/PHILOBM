using PHILOBM.Pages;
using System.Reflection;
using System.Windows;

namespace PHILOBM;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Initialiser la page de gestion des clients
        MainFrame.Navigate(new Accueil());
    }

    private void Accueil_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Accueil()); // Naviguer vers la page Accueil
    }

    private void GestionClients_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new GestionClients();
    }

    private void Apropos_Click(object sender, RoutedEventArgs e)
    {
        string version = Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "undefined";
        MessageBox.Show($"PHILO B.M - Version {version}");
    }
}
