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
using PHILOBM.Constants;
using Serilog.Events;

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
        services.AddSingleton<FileService>(provider => new FileService
        {
            DatabaseFileName = ConstantsSettings.DBName,
            BackupDirectory = ConstantsSettings.BackupPath,
            MaxBackupCount =ConstantsSettings.MaxBackupCount,
            ShowMessageBoxes = ConstantsSettings.ShowMessageBoxes
        });
        services.AddDbContext<PhiloBMContext>(options =>
                options.UseSqlite($"Data Source={ConstantsSettings.DBName}"));
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddLogging();
    }

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
