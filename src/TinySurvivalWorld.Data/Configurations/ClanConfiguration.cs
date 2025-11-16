using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Clan.
/// </summary>
public class ClanConfiguration : IEntityTypeConfiguration<Clan>
{
    public void Configure(EntityTypeBuilder<Clan> builder)
    {
        // Table name
        builder.ToTable("Clans");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasColumnType("TEXT");

        builder.Property(c => c.FactionId)
            .IsRequired(false); // Nullable pour clans indépendants

        builder.Property(c => c.EthnicityType)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(c => c.MaxMembers)
            .IsRequired()
            .HasDefaultValue(50);

        builder.Property(c => c.Tag)
            .HasMaxLength(5);

        builder.Property(c => c.FoundedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasIndex(c => c.FactionId);

        // Relationships

        // Faction (configurée dans FactionConfiguration)

        // Members
        builder.HasMany(c => c.Members)
            .WithOne(ch => ch.Clan)
            .HasForeignKey(ch => ch.ClanId)
            .OnDelete(DeleteBehavior.SetNull); // Si clan supprimé, personnages perdent leur clan

        // Ignorer les propriétés calculées
        builder.Ignore(c => c.IsIndependent);
        builder.Ignore(c => c.IsFull);
    }
}
