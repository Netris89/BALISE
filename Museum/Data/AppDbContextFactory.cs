using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Balise.Data;

/// <summary>
/// Builds an <see cref="AppDbContext"/> for design-time EF Core tools
/// (migrations, database update), which never run through the app's own
/// composition root in App.axaml.cs.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(config["Postgres:ConnectionString"],
            npgsql => npgsql.UseNetTopologySuite());

        return new AppDbContext(optionsBuilder.Options);
    }
}
