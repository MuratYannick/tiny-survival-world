using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.Models;

/// <summary>
/// Représente une faction dans le monde post-apocalyptique.
/// Les factions sont exclusives à une ethnie et regroupent des clans et des joueurs.
/// </summary>
public class Faction
{
    /// <summary>
    /// Identifiant unique de la faction.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de la faction.
    /// Exemples : "Éclaireurs de l'Aube Nouvelle", "Veilleurs de l'Ancien Monde"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description de la faction et de sa philosophie.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ethnie requise pour rejoindre cette faction.
    /// </summary>
    public Ethnicity RequiredEthnicity { get; set; }

    /// <summary>
    /// Date de création de la faction (dans le contexte du jeu).
    /// </summary>
    public DateTime FoundedDate { get; set; }

    // Navigation properties

    /// <summary>
    /// Collection des clans affiliés à cette faction.
    /// </summary>
    public ICollection<Clan> Clans { get; set; } = new List<Clan>();

    /// <summary>
    /// Collection des joueurs membres de cette faction.
    /// </summary>
    public ICollection<Player> Members { get; set; } = new List<Player>();

    /// <summary>
    /// Vérifie si un joueur peut rejoindre cette faction selon son ethnie.
    /// </summary>
    /// <param name="playerEthnicity">L'ethnie du joueur</param>
    /// <returns>True si le joueur peut rejoindre, sinon false</returns>
    public bool CanJoin(Ethnicity playerEthnicity)
    {
        return playerEthnicity == RequiredEthnicity;
    }
}
