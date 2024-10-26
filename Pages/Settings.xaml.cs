//using Microsoft.Extensions.Configuration;
//using PHILOBM;
//using System.Windows.Controls;
//using System.Windows;

//public partial class SettingsPage : Page
//{
//    public SettingsPage()
//    {
//        InitializeComponent();
//        ShowMessageBoxCheckBox.IsChecked = ((App)Application.Current).ShowMessageBoxes;
//    }

//    private void SaveButton_Click(object sender, RoutedEventArgs e)
//    {
//        // Enregistrez la valeur du CheckBox dans le fichier de configuration
//        var config = new ConfigurationBuilder()
//            .SetBasePath(AppContext.BaseDirectory)
//            .AddJsonFile("AppSettings.json", optional: false)
//            .Build();

//        config["Settings:ShowMessageBoxes"] = ShowMessageBoxCheckBox.IsChecked.ToString();

//        // Enregistrer le fichier (vous pouvez utiliser un moyen d'écriture dans un fichier JSON ici)
//        File.WriteAllText("AppSettings.json", JsonConvert.SerializeObject(config));

//        MessageBox.Show("Paramètres enregistrés.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//    }
//}
