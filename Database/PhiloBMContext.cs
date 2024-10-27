using PHILOBM.Models;
using Microsoft.EntityFrameworkCore;
using PHILOBM.Models.Base;
using PHILOBM.Constants;

namespace PHILOBM.Database;

public class PhiloBMContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Service> Services { get; set; }


    public PhiloBMContext(DbContextOptions<PhiloBMContext> options)
           : base(options)
    {
        // Assurez-vous que la base de données est créée si elle n'existe pas
        this.Database.EnsureCreated();
        //this.Database.EnsureDeleted();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={ConstantsSettings.DBName}");
    }
}

