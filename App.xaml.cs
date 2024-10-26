using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PHILOBM.Constants;

namespace PHILOBM;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    public bool ShowMessageBoxes { get; private set; }

    public App()
    {
        AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                AddServices(services);
            });

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        // Configure BackupService avec ses paramètres
        services.AddSingleton<FileService>(provider => new FileService
        {
            DatabaseFileName = Constants.Constants.DBName,
            BackupDirectory = Constants.Constants.BackupPath,
            MaxBackupCount = 1000,
            ShowMessageBoxes = false 
        });
        services.AddDbContext<PhiloBMContext>(options =>
                options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/philoBM.db"));


        services.AddScoped<IClientService, ClientService>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        AppHost!.StartAsync();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppHost!.StopAsync();
        base.OnExit(e);
    }

    private void LoadSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
            .Build();

        ShowMessageBoxes = configuration.GetValue<bool>("Settings:ShowMessageBoxes");
    }

}
