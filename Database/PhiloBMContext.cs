using PHILOBM.Models;
using Microsoft.EntityFrameworkCore;
using PHILOBM.Models.Base;

namespace PHILOBM.Database;

public class PhiloBMContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Voiture> Voitures { get; set; }
    public DbSet<Facture> Factures { get; set; }
    public DbSet<Service> Services { get; set; }

    public PhiloBMContext(DbContextOptions<PhiloBMContext> options)
           : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}

