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

        builder.Property(c => c.LeaderId)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.Name)
            .IsUnique();

        builder.HasIndex(c => c.FactionId);
        builder.HasIndex(c => c.LeaderId);

        // Relationships

        // Faction (configurée dans FactionConfiguration)
        // Leader - relation one-to-one (un joueur peut être leader d'un seul clan)
        builder.HasOne(c => c.Leader)
            .WithOne(p => p.LeadingClan)
            .HasForeignKey<Clan>(c => c.LeaderId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer le clan si le leader est supprimé

        // Members
        builder.HasMany(c => c.Members)
            .WithOne(p => p.Clan)
            .HasForeignKey(p => p.ClanId)
            .OnDelete(DeleteBehavior.SetNull); // Si clan supprimé, joueurs perdent leur clan
    }
}
