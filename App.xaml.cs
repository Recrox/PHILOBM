using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using PHILOBM.Services.Interfaces;
using System.Windows;

namespace PHILOBM;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    }

    //[STAThread]
    //public static void Main()
    //{
    //    AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    //    var app = new App();
    //    app.InitializeComponent();
    //    app.Run();
    //}

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Enregistrement des services
                services.AddSingleton<MainWindow>();
                services.AddDbContext<PhiloBMContext>();
                services.AddScoped<IClientService, ClientService>();
                // Ajoutez d'autres services ici si nécessaire
            });

    protected override void OnStartup(StartupEventArgs e)
    {
        AppHost!.StartAsync();
        // Les services sont déjà configurés dans CreateHostBuilder, vous pouvez les utiliser comme suit :
        //var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        //mainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppHost!.StopAsync();
        base.OnExit(e);
    }

}
