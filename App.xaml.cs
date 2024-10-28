using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using PHILOBM.ConstantsSettings;
using Serilog.Events;
using System.IO;

namespace PHILOBM;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    private readonly ILogger<App> _logger;

    public App()
    {
        AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        _logger = AppHost.Services.GetRequiredService<ILogger<App>>();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) => configuration
            .MinimumLevel.Error() // Définir le niveau de log à Error
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error) // Niveaux de log spécifiques pour Microsoft
            .Enrich.FromLogContext()
            .WriteTo.File("logs\\app.log", rollingInterval: RollingInterval.Day)) // Écrire uniquement dans un fichier
        .ConfigureServices((context, services) =>
        {
            AddServices(services);
        });



    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton(provider => new FileService
        {
            DatabaseFileName = Constants.DBName,
            BackupDirectory = Constants.BackupFolderName,
            MaxBackupCount = Constants.MaxBackupCount,
            ShowMessageBoxes = Constants.ShowMessageBoxes
        });
        AddDbContextRelative(services);

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddLogging();
    }

    private static void AddDbContextRelative(IServiceCollection services)
    {
        Outils.CréerDossierSiInexistant(Constants.RacinePath);

        services.AddDbContext<PhiloBMContext>(options =>
            options.UseSqlite($"Data Source={Constants.DbPath}"));
    }

    //private static void AddDbContextAbsolu(IServiceCollection services)
    //{
    //    // Dossier de l'application dans le dossier AppData\Roaming de l'utilisateur
    //    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    //    var dbDirectory = Path.Combine(appDataPath, "PhiloBM", "Database");

    //    // Créer le répertoire s'il n'existe pas
    //    if (!Directory.Exists(dbDirectory))
    //    {
    //        Directory.CreateDirectory(dbDirectory);
    //    }

    //    // Chemin absolu pour la base de données
    //    var dbPath = Path.Combine(dbDirectory, ConstantsSettings.DBName);

    //    services.AddDbContext<PhiloBMContext>(options =>
    //        options.UseSqlite($"Data Source={dbPath}"));
    //}


    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            AppHost!.StartAsync();
            LoadSettings();
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
            AppHost!.StopAsync().Wait();
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
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .Build();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Une erreur s'est produite lors du chargement des paramètres.");
        }
    }
}
