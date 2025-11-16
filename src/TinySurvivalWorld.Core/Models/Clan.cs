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
    /// Tag/sigle du clan (optionnel, 2-5 caractères).
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Date de création du clan.
    /// </summary>
    public DateTime FoundedDate { get; set; }

    // Navigation properties

    /// <summary>
    /// Faction à laquelle le clan est affilié (null si indépendant).
    /// </summary>
    public Faction? Faction { get; set; }

    /// <summary>
    /// Collection des membres du clan (joueurs et PNJ).
    /// </summary>
    public ICollection<Character> Members { get; set; } = new List<Character>();

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
    /// Vérifie si un personnage peut rejoindre ce clan selon les règles de recrutement.
    /// </summary>
    /// <param name="character">Le personnage candidat</param>
    /// <returns>True si le personnage peut rejoindre, sinon false</returns>
    public bool CanJoin(Character character)
    {
        // Le clan est plein
        if (IsFull)
            return false;

        // Clan affilié à une faction
        if (!IsIndependent)
        {
            // Le personnage doit être membre de la même faction
            return character.FactionId == FactionId;
        }

        // Clan indépendant
        // Le personnage ne doit pas être membre d'une faction
        if (character.FactionId.HasValue)
            return false;

        // Vérifier les restrictions ethniques
        return EthnicityType switch
        {
            ClanEthnicityType.AwakenedOnly => character.Ethnicity == Ethnicity.Awakened,
            ClanEthnicityType.UnalteredOnly => character.Ethnicity == Ethnicity.Unaltered,
            ClanEthnicityType.Mixed => true,
            _ => false
        };
    }
}
