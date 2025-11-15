using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Player.
/// </summary>
public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        // Table name
        builder.ToTable("Players");

        // Primary key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Ethnicity)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(p => p.FactionId)
            .IsRequired(false);

        builder.Property(p => p.ClanId)
            .IsRequired(false);

        builder.Property(p => p.WorldId)
            .IsRequired();

        builder.Property(p => p.Level)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(p => p.Experience)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.Health)
            .IsRequired()
            .HasDefaultValue(100f);

        builder.Property(p => p.MaxHealth)
            .IsRequired()
            .HasDefaultValue(100f);

        builder.Property(p => p.Hunger)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(p => p.Thirst)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(p => p.PositionX)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(p => p.PositionY)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Property(p => p.LastLogin)
            .IsRequired();

        // Indexes
        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasIndex(p => p.FactionId);
        builder.HasIndex(p => p.ClanId);
        builder.HasIndex(p => p.WorldId);

        // Relationships

        // World
        builder.HasOne(p => p.World)
            .WithMany(w => w.Players)
            .HasForeignKey(p => p.WorldId)
            .OnDelete(DeleteBehavior.Cascade); // Si monde supprimé, joueurs aussi

        // Faction (configurée dans FactionConfiguration)
        // Clan (configurée dans ClanConfiguration)
        // LeadingClan (configurée dans ClanConfiguration)

        // Ignorer les propriétés calculées
        builder.Ignore(p => p.HasFaction);
        builder.Ignore(p => p.HasClan);
        builder.Ignore(p => p.IsAlive);
    }
}
