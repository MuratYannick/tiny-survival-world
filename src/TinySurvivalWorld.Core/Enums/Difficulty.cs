namespace TinySurvivalWorld.Core.Enums;

/// <summary>
/// Niveau de difficulté du monde.
/// </summary>
public enum Difficulty
{
    /// <summary>
    /// Facile - Ressources abondantes, ennemis faibles.
    /// </summary>
    Easy = 0,

    /// <summary>
    /// Normal - Équilibré entre challenge et accessibilité.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Difficile - Ressources rares, ennemis puissants, survie intense.
    /// </summary>
    Hard = 2,

    /// <summary>
    /// Hardcore - Permadeath, conditions extrêmes.
    /// </summary>
    Hardcore = 3
}
