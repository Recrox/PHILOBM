using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PHILOBM.Database;
using PHILOBM.Services;
using System.Windows;

namespace PHILOBM
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        [STAThread]
        public static void Main()
        {
            AppHost = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Enregistrement des services
                    services.AddTransient<MainWindow>();
                    services.AddDbContext<PhiloBMContext>();
                    services.AddScoped<IClientService, ClientService>();
                    // Ajoutez d'autres services ici si nécessaire
                });

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Les services sont déjà configurés dans CreateHostBuilder, vous pouvez les utiliser comme suit :
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
