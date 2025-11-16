using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TinySurvivalWorld.Data;

/// <summary>
/// Factory pour créer GameDbContext au moment du design (pour les migrations EF Core).
/// </summary>
public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
{
    public GameDbContext CreateDbContext(string[] args)
    {
        // Construire la configuration depuis appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TinySurvivalWorld.Game.Desktop"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // Récupérer la connection string
        var connectionString = configuration.GetConnectionString("GameDatabase");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'GameDatabase' not found in appsettings.json");
        }

        // Configurer les options du DbContext
        var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();

        // Utiliser MySQL 8.0 par défaut pour les migrations (évite de nécessiter une connexion active)
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

        optionsBuilder.UseMySql(
            connectionString,
            serverVersion,
            options =>
            {
                options.MigrationsAssembly(typeof(GameDbContext).Assembly.FullName);
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            });

        return new GameDbContext(optionsBuilder.Options);
    }
}
