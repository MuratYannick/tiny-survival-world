namespace TinySurvivalWorld.Core.Time;

/// <summary>
/// Périodes de la journée pour le cycle jour/nuit.
/// </summary>
public enum TimeOfDay
{
    /// <summary>
    /// Nuit profonde (00:00 - 05:00)
    /// </summary>
    Night,

    /// <summary>
    /// Aube / Lever du soleil (05:00 - 07:00)
    /// </summary>
    Dawn,

    /// <summary>
    /// Matin (07:00 - 12:00)
    /// </summary>
    Morning,

    /// <summary>
    /// Après-midi (12:00 - 17:00)
    /// </summary>
    Afternoon,

    /// <summary>
    /// Crépuscule / Coucher du soleil (17:00 - 19:00)
    /// </summary>
    Dusk,

    /// <summary>
    /// Soirée (19:00 - 00:00)
    /// </summary>
    Evening
}
