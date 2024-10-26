using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Pages;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PHILOBM;

public partial class MainWindow : Window
{
    private const string DatabaseFileName = Constants.ConstantsSettings.DBName; // Nom de votre fichier de base de données
    private const string BackupDirectory = Constants.ConstantsSettings.BackupPath; // Dossier de sauvegarde
    private const int MaxBackupCount = 1;
    public bool ShowMessageBoxes { get; set; }

    private readonly FileService _backupService;

    public MainWindow()
    {
        InitializeComponent();
        // Initialiser la page de gestion des clients
        var fileService = App.AppHost?.Services.GetRequiredService<FileService>() ?? throw new Exception("FileService not loaded"); ;
        _backupService = fileService;

        MainFrame.Navigate(new Accueil());
        this.Closing += MainWindow_Closing; // Gérer l'événement de fermeture
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

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        _backupService.SauvegarderBaseDeDonnees();
    }
}