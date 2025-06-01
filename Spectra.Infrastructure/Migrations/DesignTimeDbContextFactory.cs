using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Spectra.Infrastructure.Data;

namespace Spectra.Infrastructure.Migrations;

/// <summary>
/// Design-time factory for creating ApplicationDbContext instances during migrations
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use a default connection string for migrations
        // This will be overridden at runtime by the actual configuration
        var connectionString = "Host=localhost;Database=spectra_db;Username=postgres;Password=postgres;Port=5432";
        
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Spectra.Infrastructure");
        });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
