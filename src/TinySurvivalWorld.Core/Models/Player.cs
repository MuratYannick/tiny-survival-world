using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.Models;

/// <summary>
/// Représente un joueur dans le monde de Tiny Survival World.
/// </summary>
public class Player
{
    /// <summary>
    /// Identifiant unique du joueur.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du joueur (unique).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ethnie du joueur (Éveillés ou Inaltérés).
    /// Détermine quelle faction le joueur peut rejoindre.
    /// </summary>
    public Ethnicity Ethnicity { get; set; }

    /// <summary>
    /// Identifiant de la faction à laquelle le joueur appartient (null si aucune).
    /// </summary>
    public int? FactionId { get; set; }

    /// <summary>
    /// Identifiant du clan auquel le joueur appartient (null si aucun).
    /// </summary>
    public Guid? ClanId { get; set; }

    /// <summary>
    /// Identifiant du monde dans lequel le joueur existe.
    /// </summary>
    public Guid WorldId { get; set; }

    // Statistiques de base

    /// <summary>
    /// Niveau du joueur.
    /// </summary>
    public int Level { get; set; } = 1;

    /// <summary>
    /// Points d'expérience actuels.
    /// </summary>
    public int Experience { get; set; } = 0;

    /// <summary>
    /// Santé actuelle.
    /// </summary>
    public float Health { get; set; } = 100f;

    /// <summary>
    /// Santé maximale.
    /// </summary>
    public float MaxHealth { get; set; } = 100f;

    /// <summary>
    /// Niveau de faim (0-100, 100 = affamé).
    /// </summary>
    public float Hunger { get; set; } = 0f;

    /// <summary>
    /// Niveau de soif (0-100, 100 = assoiffé).
    /// </summary>
    public float Thirst { get; set; } = 0f;

    /// <summary>
    /// Position X dans le monde.
    /// </summary>
    public float PositionX { get; set; } = 0f;

    /// <summary>
    /// Position Y dans le monde.
    /// </summary>
    public float PositionY { get; set; } = 0f;

    // Métadonnées

    /// <summary>
    /// Date de création du personnage.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Dernière mise à jour du personnage.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Dernière connexion du joueur.
    /// </summary>
    public DateTime LastLogin { get; set; }

    // Navigation properties

    /// <summary>
    /// Faction à laquelle le joueur appartient (null si aucune).
    /// </summary>
    public Faction? Faction { get; set; }

    /// <summary>
    /// Clan auquel le joueur appartient (null si aucun).
    /// </summary>
    public Clan? Clan { get; set; }

    /// <summary>
    /// Clan dirigé par ce joueur (null s'il n'est pas leader).
    /// </summary>
    public Clan? LeadingClan { get; set; }

    /// <summary>
    /// Monde dans lequel le joueur existe.
    /// </summary>
    public World World { get; set; } = null!;

    // Business logic

    /// <summary>
    /// Indique si le joueur est membre d'une faction.
    /// </summary>
    public bool HasFaction => FactionId.HasValue;

    /// <summary>
    /// Indique si le joueur est membre d'un clan.
    /// </summary>
    public bool HasClan => ClanId.HasValue;

    /// <summary>
    /// Indique si le joueur est vivant.
    /// </summary>
    public bool IsAlive => Health > 0;

    /// <summary>
    /// Vérifie si le joueur peut rejoindre une faction spécifique.
    /// </summary>
    /// <param name="faction">La faction à rejoindre</param>
    /// <returns>True si le joueur peut rejoindre, sinon false</returns>
    public bool CanJoinFaction(Faction faction)
    {
        // Déjà membre d'une faction
        if (HasFaction)
            return false;

        // Vérifier l'ethnie
        return faction.CanJoin(Ethnicity);
    }

    /// <summary>
    /// Vérifie si le joueur peut rejoindre un clan spécifique.
    /// </summary>
    /// <param name="clan">Le clan à rejoindre</param>
    /// <returns>True si le joueur peut rejoindre, sinon false</returns>
    public bool CanJoinClan(Clan clan)
    {
        // Déjà membre d'un clan
        if (HasClan)
            return false;

        // Vérifier les règles du clan
        return clan.CanJoin(this);
    }

    /// <summary>
    /// Applique des dégâts au joueur.
    /// </summary>
    /// <param name="damage">Montant des dégâts</param>
    public void TakeDamage(float damage)
    {
        Health = Math.Max(0, Health - damage);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Soigne le joueur.
    /// </summary>
    /// <param name="amount">Montant de soin</param>
    public void Heal(float amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Ajoute de l'expérience au joueur.
    /// </summary>
    /// <param name="xp">Montant d'expérience</param>
    public void GainExperience(int xp)
    {
        Experience += xp;
        UpdatedAt = DateTime.UtcNow;

        // TODO: Implémenter la logique de level up
    }
}
