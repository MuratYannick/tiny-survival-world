using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Faction.
/// </summary>
public class FactionConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        // Table name
        builder.ToTable("Factions");

        // Primary key
        builder.HasKey(f => f.Id);

        // Properties
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Description)
            .IsRequired()
            .HasColumnType("TEXT");

        builder.Property(f => f.RequiredEthnicity)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(f => f.FoundedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(f => f.Name)
            .IsUnique();

        // Relationships
        builder.HasMany(f => f.Clans)
            .WithOne(c => c.Faction)
            .HasForeignKey(c => c.FactionId)
            .OnDelete(DeleteBehavior.SetNull); // Si faction supprimée, clans deviennent indépendants

        builder.HasMany(f => f.Members)
            .WithOne(ch => ch.Faction)
            .HasForeignKey(ch => ch.FactionId)
            .OnDelete(DeleteBehavior.SetNull); // Si faction supprimée, personnages perdent leur faction
    }
}
