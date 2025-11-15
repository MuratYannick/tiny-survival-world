using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.Models;

/// <summary>
/// Représente un clan dans le jeu.
/// Un clan peut être affilié à une faction ou être indépendant.
/// </summary>
public class Clan
{
    /// <summary>
    /// Identifiant unique du clan.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du clan.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description du clan, sa philosophie et ses objectifs.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant de la faction à laquelle le clan est affilié (null si indépendant).
    /// Les clans affiliés à une faction ne recrutent que des membres de cette faction.
    /// </summary>
    public int? FactionId { get; set; }

    /// <summary>
    /// Type de restriction ethnique pour le recrutement.
    /// Utilisé uniquement pour les clans indépendants (FactionId == null).
    /// </summary>
    public ClanEthnicityType EthnicityType { get; set; }

    /// <summary>
    /// Nombre maximum de membres autorisés dans le clan.
    /// </summary>
    public int MaxMembers { get; set; } = 50;

    /// <summary>
    /// Date de création du clan.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identifiant du joueur fondateur/leader du clan.
    /// </summary>
    public Guid LeaderId { get; set; }

    // Navigation properties

    /// <summary>
    /// Faction à laquelle le clan est affilié (null si indépendant).
    /// </summary>
    public Faction? Faction { get; set; }

    /// <summary>
    /// Leader du clan.
    /// </summary>
    public Player Leader { get; set; } = null!;

    /// <summary>
    /// Collection des membres du clan.
    /// </summary>
    public ICollection<Player> Members { get; set; } = new List<Player>();

    // Business logic

    /// <summary>
    /// Indique si le clan est indépendant (non affilié à une faction).
    /// </summary>
    public bool IsIndependent => FactionId == null;

    /// <summary>
    /// Indique si le clan a atteint sa capacité maximale.
    /// </summary>
    public bool IsFull => Members.Count >= MaxMembers;

    /// <summary>
    /// Vérifie si un joueur peut rejoindre ce clan selon les règles de recrutement.
    /// </summary>
    /// <param name="player">Le joueur candidat</param>
    /// <returns>True si le joueur peut rejoindre, sinon false</returns>
    public bool CanJoin(Player player)
    {
        // Le clan est plein
        if (IsFull)
            return false;

        // Clan affilié à une faction
        if (!IsIndependent)
        {
            // Le joueur doit être membre de la même faction
            return player.FactionId == FactionId;
        }

        // Clan indépendant
        // Le joueur ne doit pas être membre d'une faction
        if (player.FactionId.HasValue)
            return false;

        // Vérifier les restrictions ethniques
        return EthnicityType switch
        {
            ClanEthnicityType.AwakenedOnly => player.Ethnicity == Ethnicity.Awakened,
            ClanEthnicityType.UnalteredOnly => player.Ethnicity == Ethnicity.Unaltered,
            ClanEthnicityType.Mixed => true,
            _ => false
        };
    }
}
