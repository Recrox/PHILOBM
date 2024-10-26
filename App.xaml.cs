using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PHILOBM.Constants;

namespace PHILOBM
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        private readonly ILogger<App> _logger;

        public App()
        {
            AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
            _logger = AppHost.Services.GetRequiredService<ILogger<App>>(); // Initialiser le logger
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole(); // Ajoutez d'autres fournisseurs si nécessaire
                })
                .ConfigureServices((context, services) =>
                {
                    AddServices(services);
                });

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<FileService>(provider => new FileService
            {
                DatabaseFileName = Constants.Constants.DBName,
                BackupDirectory = Constants.Constants.BackupPath,
                MaxBackupCount = Constants.Constants.MaxBackupCount,
                ShowMessageBoxes = Constants.Constants.ShowMessageBoxes
            });
            services.AddDbContext<PhiloBMContext>(options =>
                    options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/philoBM.db"));
            services.AddScoped<IClientService, ClientService>();
            services.AddLogging(); // Ajouter les services de journalisation
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                AppHost!.StartAsync();
                LoadSettings(); // Charger les paramètres ici
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors du démarrage de l'application.");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                AppHost!.StopAsync().Wait(); // Attendre que l'arrêt soit complet
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de l'arrêt de l'application.");
            }
        }

        private void LoadSettings()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                //ShowMessageBoxes = configuration.GetValue<bool>("Settings:ShowMessageBoxes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors du chargement des paramètres.");
            }
        }
    }
}
