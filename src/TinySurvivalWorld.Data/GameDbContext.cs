using Microsoft.EntityFrameworkCore;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data;

/// <summary>
/// Contexte de base de données Entity Framework Core pour Tiny Survival World.
/// </summary>
public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }

    // DbSets - Tables de la base de données

    /// <summary>
    /// Table des factions.
    /// </summary>
    public DbSet<Faction> Factions { get; set; } = null!;

    /// <summary>
    /// Table des clans.
    /// </summary>
    public DbSet<Clan> Clans { get; set; } = null!;

    /// <summary>
    /// Table des joueurs.
    /// </summary>
    public DbSet<Player> Players { get; set; } = null!;

    /// <summary>
    /// Table des mondes.
    /// </summary>
    public DbSet<World> Worlds { get; set; } = null!;

    /// <summary>
    /// Table du catalogue d'items.
    /// </summary>
    public DbSet<Item> Items { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Application des configurations depuis les classes de configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDbContext).Assembly);
    }
}
