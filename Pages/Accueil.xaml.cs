using PHILOBM.Constants;
using PHILOBM.Pages;
using PHILOBM.Services;
using System.Diagnostics;
using System.IO;
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

    private void OpenDbLocation_Click(object sender, RoutedEventArgs e)
    {
        //string dbPath = Path.Combine(ConstantsSettings.RacinePath, ConstantsSettings.DBName);
        Outils.OpenFolder(ConstantsSettings.RacinePath);
    }
}
