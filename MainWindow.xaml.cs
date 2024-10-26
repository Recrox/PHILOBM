using PHILOBM.Pages;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PHILOBM;

public partial class MainWindow : Window
{
    private const string DatabaseFileName = "philoBM.db"; // Nom de votre fichier de base de données
    private const string BackupDirectory = "Backups"; // Dossier de sauvegarde
    private const int MaxBackupCount = 1;
    public bool ShowMessageBoxes { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        // Initialiser la page de gestion des clients
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

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SauvegarderBaseDeDonnees();
    }

    private void SauvegarderBaseDeDonnees()
    {
        try
        {
            // Créer le dossier de sauvegarde s'il n'existe pas
            if (!Directory.Exists(BackupDirectory))
            {
                Directory.CreateDirectory(BackupDirectory);
            }

            // Chemin complet de la base de données actuelle
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

            // Créer un nom de fichier de sauvegarde unique
            string backupFileName = Path.Combine(BackupDirectory, $"{DatabaseFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

            // Copier le fichier de base de données
            File.Copy(sourceFile, backupFileName, true); // true pour écraser le fichier s'il existe

            // Gérer les sauvegardes excédentaires
            GérerSauvegardesExcédentaires();

            // Afficher le MessageBox si ShowMessageBoxes est vrai
            if (ShowMessageBoxes)
            {
                MessageBox.Show($"Sauvegarde réussie : {backupFileName}", "Sauvegarde", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la sauvegarde de la base de données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void GérerSauvegardesExcédentaires()
    {
        // Récupérer tous les fichiers de sauvegarde
        var backupFiles = Directory.GetFiles(BackupDirectory)
            .OrderBy(f => f) // Tri par nom de fichier (les plus anciens en premier)
            .ToList();

        // Vérifier si le nombre de fichiers dépasse la constante MaxBackupCount
        if (backupFiles.Count > MaxBackupCount)
        {
            // Supprimer les fichiers les plus anciens
            int filesToDelete = backupFiles.Count - MaxBackupCount;
            for (int i = 0; i < filesToDelete; i++)
            {
                File.Delete(backupFiles[i]);
            }

            // Afficher le MessageBox si ShowMessageBoxes est vrai
            if (ShowMessageBoxes)
            {
                MessageBox.Show($"{filesToDelete} sauvegardes supprimées pour maintenir le nombre à {MaxBackupCount}.", "Gestion des Sauvegardes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

}
