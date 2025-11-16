namespace TinySurvivalWorld.Core.Enums;

/// <summary>
/// Définit les restrictions ethniques pour le recrutement d'un clan.
/// </summary>
public enum ClanEthnicityType
{
    /// <summary>
    /// Clan exclusif aux Éveillés.
    /// </summary>
    AwakenedOnly = 0,

    /// <summary>
    /// Clan exclusif aux Inaltérés.
    /// </summary>
    UnalteredOnly = 1,

    /// <summary>
    /// Clan mixte acceptant les deux ethnies.
    /// </summary>
    Mixed = 2
}
