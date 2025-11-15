namespace TinySurvivalWorld.Core.Enums;

/// <summary>
/// Type d'item dans le jeu.
/// </summary>
public enum ItemType
{
    /// <summary>
    /// Ressource brute (bois, pierre, minerai, etc.).
    /// </summary>
    Resource = 0,

    /// <summary>
    /// Outil (hache, pioche, pelle, etc.).
    /// </summary>
    Tool = 1,

    /// <summary>
    /// Arme (épée, arc, fusil, etc.).
    /// </summary>
    Weapon = 2,

    /// <summary>
    /// Armure ou vêtement (casque, plastron, bottes, etc.).
    /// </summary>
    Armor = 3,

    /// <summary>
    /// Nourriture (pain, viande, fruits, etc.).
    /// </summary>
    Food = 4,

    /// <summary>
    /// Boisson (eau, potion, etc.).
    /// </summary>
    Drink = 5,

    /// <summary>
    /// Objet de crafting ou composant.
    /// </summary>
    CraftingMaterial = 6,

    /// <summary>
    /// Consommable (potion de soin, bandage, etc.).
    /// </summary>
    Consumable = 7,

    /// <summary>
    /// Objet de quête.
    /// </summary>
    QuestItem = 8,

    /// <summary>
    /// Munition (flèches, balles, etc.).
    /// </summary>
    Ammunition = 9,

    /// <summary>
    /// Autre (divers objets non catégorisés).
    /// </summary>
    Misc = 99
}
