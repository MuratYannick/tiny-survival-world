using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité World.
/// </summary>
public class WorldConfiguration : IEntityTypeConfiguration<World>
{
    public void Configure(EntityTypeBuilder<World> builder)
    {
        // Table name
        builder.ToTable("Worlds");

        // Primary key
        builder.HasKey(w => w.Id);

        // Properties
        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Seed)
            .IsRequired();

        builder.Property(w => w.GameTime)
            .IsRequired()
            .HasDefaultValue(0L);

        builder.Property(w => w.Difficulty)
            .IsRequired()
            .HasConversion<byte>()
            .HasDefaultValue(Core.Enums.Difficulty.Normal);

        builder.Property(w => w.IsHardcore)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(w => w.WorldSizeX)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(w => w.WorldSizeY)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(w => w.SpawnPointX)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(w => w.SpawnPointY)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(w => w.GameVersion)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("0.1.0");

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.LastPlayed)
            .IsRequired();

        // Indexes
        builder.HasIndex(w => w.Name)
            .IsUnique();

        // Relationships
        builder.HasMany(w => w.Characters)
            .WithOne(ch => ch.World)
            .HasForeignKey(ch => ch.WorldId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignorer les propriétés calculées
        builder.Ignore(w => w.IsInfinite);
        builder.Ignore(w => w.CharacterCount);
        builder.Ignore(w => w.PlayerCount);
        builder.Ignore(w => w.GameTimeSpan);
    }
}
