using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinySurvivalWorld.Core.Models;

namespace TinySurvivalWorld.Data.Configurations;

/// <summary>
/// Configuration EF Core pour l'entité Item.
/// </summary>
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Table name
        builder.ToTable("Items");

        // Primary key
        builder.HasKey(i => i.Id);

        // Properties
        builder.Property(i => i.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Description)
            .HasColumnType("TEXT");

        builder.Property(i => i.Type)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(i => i.IsStackable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(i => i.MaxStackSize)
            .IsRequired()
            .HasDefaultValue(99);

        builder.Property(i => i.Weight)
            .IsRequired()
            .HasDefaultValue(1f);

        builder.Property(i => i.BaseValue)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(i => i.IconPath)
            .HasMaxLength(255);

        builder.Property(i => i.MaxDurability)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(i => i.Damage)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(i => i.Defense)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(i => i.HealthRestore)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(i => i.HungerRestore)
            .IsRequired()
            .HasDefaultValue(0f);

        builder.Property(i => i.ThirstRestore)
            .IsRequired()
            .HasDefaultValue(0f);

        // Indexes
        builder.HasIndex(i => i.Code)
            .IsUnique();

        builder.HasIndex(i => i.Type);

        // Ignorer les propriétés calculées
        builder.Ignore(i => i.HasDurability);
        builder.Ignore(i => i.IsWeapon);
        builder.Ignore(i => i.IsTool);
        builder.Ignore(i => i.IsConsumable);
        builder.Ignore(i => i.IsEquippable);
    }
}
