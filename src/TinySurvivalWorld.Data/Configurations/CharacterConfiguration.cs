using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Character.
/// </summary>
public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        // Table name
        builder.ToTable("Characters");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.IsPlayer)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.Ethnicity)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(c => c.FactionId)
            .IsRequired(false);

        builder.Property(c => c.ClanId)
            .IsRequired(false);

        builder.Property(c => c.WorldId)
            .IsRequired();

        builder.Property(c => c.IsClanLeader)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.IsFactionLeader)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.Level)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(c => c.Experience)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(c => c.Health)
            .IsRequired()
            .HasDefaultValue(100f);

        builder.Property(c => c.MaxHealth)
            .IsRequired()
            .HasDefaultValue(100f);

        builder.Property(c => c.Hunger)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(c => c.Thirst)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(c => c.PositionX)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(c => c.PositionY)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.Property(c => c.LastLogin)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasIndex(c => c.IsPlayer);
        builder.HasIndex(c => c.FactionId);
        builder.HasIndex(c => c.ClanId);
        builder.HasIndex(c => c.WorldId);
        builder.HasIndex(c => c.IsClanLeader);
        builder.HasIndex(c => c.IsFactionLeader);

        // Relationships

        // World
        builder.HasOne(c => c.World)
            .WithMany(w => w.Characters)
            .HasForeignKey(c => c.WorldId)
            .OnDelete(DeleteBehavior.Cascade); // Si monde supprimé, personnages aussi

        // Faction (configurée dans FactionConfiguration)
        // Clan (configurée dans ClanConfiguration)

        // Ignorer les propriétés calculées
        builder.Ignore(c => c.HasFaction);
        builder.Ignore(c => c.HasClan);
        builder.Ignore(c => c.IsAlive);
    }
}
