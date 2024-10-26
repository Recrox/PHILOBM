using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace PHILOBM;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

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
        services.AddDbContext<PhiloBMContext>(options =>
            //options.UseSqlite(context.Configuration.GetConnectionString("SQLiteDefault")));
            options.UseSqlite("Data Source=philoBM.db"));

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

}
