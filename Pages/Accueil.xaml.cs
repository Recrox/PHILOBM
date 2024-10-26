using PHILOBM.Pages;
using System.Windows;
using System.Windows.Controls;

namespace PHILOBM.Pages;

public partial class Accueil : Page
{
    public Accueil()
    {
        InitializeComponent();
    }

    private void Commencer_Click(object sender, RoutedEventArgs e)
    {
        // Navigue vers la page de gestion des clients
        NavigationService.Navigate(new GestionClients());
    }
}
