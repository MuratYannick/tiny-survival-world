using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.Models;

/// <summary>
/// Représente un type d'item dans le jeu (catalogue).
/// Il s'agit du template/définition d'item, pas d'une instance dans l'inventaire.
/// </summary>
public class Item
{
    /// <summary>
    /// Identifiant unique de l'item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Code unique de l'item (identifiant textuel, ex: "wood_plank", "iron_sword").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Nom affiché de l'item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description de l'item.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type d'item (Resource, Tool, Weapon, etc.).
    /// </summary>
    public ItemType Type { get; set; }

    /// <summary>
    /// Indique si l'item peut être empilé dans l'inventaire.
    /// </summary>
    public bool IsStackable { get; set; } = true;

    /// <summary>
    /// Nombre maximum d'items dans une pile (si stackable).
    /// </summary>
    public int MaxStackSize { get; set; } = 99;

    /// <summary>
    /// Poids unitaire de l'item (pour futur système de poids d'inventaire).
    /// </summary>
    public float Weight { get; set; } = 1f;

    /// <summary>
    /// Valeur marchande de base de l'item (pour le commerce).
    /// </summary>
    public int BaseValue { get; set; } = 0;

    /// <summary>
    /// Chemin vers l'icône de l'item dans les assets.
    /// </summary>
    public string? IconPath { get; set; }

    // Propriétés spécifiques aux outils/armes

    /// <summary>
    /// Durabilité maximale de l'item (0 = indestructible).
    /// Applicable aux outils, armes, armures.
    /// </summary>
    public int MaxDurability { get; set; } = 0;

    /// <summary>
    /// Dégâts infligés par l'item (pour les armes).
    /// </summary>
    public float Damage { get; set; } = 0f;

    /// <summary>
    /// Défense apportée par l'item (pour les armures).
    /// </summary>
    public float Defense { get; set; } = 0f;

    // Propriétés spécifiques aux consommables

    /// <summary>
    /// Points de santé restaurés à la consommation.
    /// </summary>
    public float HealthRestore { get; set; } = 0f;

    /// <summary>
    /// Points de faim réduits à la consommation.
    /// </summary>
    public float HungerRestore { get; set; } = 0f;

    /// <summary>
    /// Points de soif réduits à la consommation.
    /// </summary>
    public float ThirstRestore { get; set; } = 0f;

    // Business logic

    /// <summary>
    /// Indique si l'item a une durabilité.
    /// </summary>
    public bool HasDurability => MaxDurability > 0;

    /// <summary>
    /// Indique si l'item est une arme.
    /// </summary>
    public bool IsWeapon => Type == ItemType.Weapon;

    /// <summary>
    /// Indique si l'item est un outil.
    /// </summary>
    public bool IsTool => Type == ItemType.Tool;

    /// <summary>
    /// Indique si l'item est comestible.
    /// </summary>
    public bool IsConsumable => Type == ItemType.Food || Type == ItemType.Drink || Type == ItemType.Consumable;

    /// <summary>
    /// Indique si l'item peut être équipé.
    /// </summary>
    public bool IsEquippable => Type == ItemType.Weapon || Type == ItemType.Tool || Type == ItemType.Armor;
}
