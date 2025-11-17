namespace TinySurvivalWorld.Core.Time;

/// <summary>
/// Gestionnaire du temps de jeu avec horloge continue (même hors ligne).
/// Le temps s'écoule en permanence basé sur l'horloge réelle.
///
/// Ratio : 1 journée IG (24h) = 20 heures IRL
/// Date d'initialisation : 01/01/2025 00:00:00 UTC IRL → 01/01/2125 00:00:00 UTC IG
/// </summary>
public class TimeManager
{
    // ========================================
    // CONSTANTES DE CONFIGURATION
    // ========================================

    /// <summary>
    /// Date de référence IRL (Real Life) : 01/01/2025 00:00:00 UTC
    /// </summary>
    private static readonly DateTime EpochRealTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Date de référence IG (In Game) : 01/01/2125 00:00:00 UTC
    /// </summary>
    private static readonly DateTime EpochGameTime = new DateTime(2125, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Ratio de conversion du temps : 1 jour IG (24h) = 20h IRL
    /// Donc : temps_IG = temps_IRL × (24/20) = temps_IRL × 1.2
    /// </summary>
    private const double TimeRatio = 24.0 / 20.0; // = 1.2

    // ========================================
    // PROPRIÉTÉS PUBLIQUES
    // ========================================

    /// <summary>
    /// Date et heure actuelles dans le jeu (calculée en temps réel).
    /// </summary>
    public DateTime CurrentGameTime
    {
        get
        {
            // Calcul du temps écoulé IRL depuis l'époch
            TimeSpan elapsedRealTime = DateTime.UtcNow - EpochRealTime;

            // Conversion en temps IG (multiplié par le ratio)
            TimeSpan elapsedGameTime = TimeSpan.FromTicks((long)(elapsedRealTime.Ticks * TimeRatio));

            // Date actuelle IG = époch IG + temps écoulé IG
            return EpochGameTime.Add(elapsedGameTime);
        }
    }

    /// <summary>
    /// Numéro du jour actuel depuis le début du jeu (jour 1 = 01/01/2125).
    /// </summary>
    public int CurrentDay
    {
        get
        {
            TimeSpan elapsed = CurrentGameTime - EpochGameTime;
            return (int)elapsed.TotalDays + 1; // +1 car on commence au jour 1
        }
    }

    /// <summary>
    /// Heure actuelle de la journée (0-23).
    /// </summary>
    public int CurrentHour => CurrentGameTime.Hour;

    /// <summary>
    /// Minute actuelle (0-59).
    /// </summary>
    public int CurrentMinute => CurrentGameTime.Minute;

    /// <summary>
    /// Seconde actuelle (0-59).
    /// </summary>
    public int CurrentSecond => CurrentGameTime.Second;

    /// <summary>
    /// Période de la journée actuelle (Nuit, Aube, Jour, etc.).
    /// </summary>
    public TimeOfDay CurrentTimeOfDay
    {
        get
        {
            int hour = CurrentHour;

            return hour switch
            {
                >= 0 and < 5 => TimeOfDay.Night,
                >= 5 and < 7 => TimeOfDay.Dawn,
                >= 7 and < 12 => TimeOfDay.Morning,
                >= 12 and < 17 => TimeOfDay.Afternoon,
                >= 17 and < 19 => TimeOfDay.Dusk,
                >= 19 and < 24 => TimeOfDay.Evening,
                _ => TimeOfDay.Night
            };
        }
    }

    /// <summary>
    /// Indique si c'est actuellement la journée (Dawn, Morning, Afternoon).
    /// </summary>
    public bool IsDay => CurrentTimeOfDay is TimeOfDay.Dawn or TimeOfDay.Morning or TimeOfDay.Afternoon;

    /// <summary>
    /// Indique si c'est actuellement la nuit (Night, Evening).
    /// </summary>
    public bool IsNight => CurrentTimeOfDay is TimeOfDay.Night or TimeOfDay.Evening;

    /// <summary>
    /// Indique si c'est actuellement un crépuscule (Dawn ou Dusk).
    /// </summary>
    public bool IsTwilight => CurrentTimeOfDay is TimeOfDay.Dawn or TimeOfDay.Dusk;

    // ========================================
    // ÉVÉNEMENTS
    // ========================================

    /// <summary>
    /// Déclenché quand l'heure change (chaque heure IG).
    /// </summary>
    public event EventHandler<int>? OnHourChanged;

    /// <summary>
    /// Déclenché quand le jour change (chaque nouveau jour IG).
    /// </summary>
    public event EventHandler<int>? OnDayChanged;

    /// <summary>
    /// Déclenché quand la période de la journée change (Nuit → Aube → Jour, etc.).
    /// </summary>
    public event EventHandler<TimeOfDay>? OnTimeOfDayChanged;

    // ========================================
    // VARIABLES INTERNES
    // ========================================

    private int _lastHour = -1;
    private int _lastDay = -1;
    private TimeOfDay _lastTimeOfDay = TimeOfDay.Night;

    // ========================================
    // MÉTHODES PUBLIQUES
    // ========================================

    /// <summary>
    /// Constructeur du TimeManager.
    /// Initialise les valeurs de tracking.
    /// </summary>
    public TimeManager()
    {
        _lastHour = CurrentHour;
        _lastDay = CurrentDay;
        _lastTimeOfDay = CurrentTimeOfDay;
    }

    /// <summary>
    /// Met à jour le TimeManager (à appeler chaque frame).
    /// Vérifie les changements d'heure/jour/période et déclenche les événements.
    /// </summary>
    public void Update()
    {
        int currentHour = CurrentHour;
        int currentDay = CurrentDay;
        TimeOfDay currentTimeOfDay = CurrentTimeOfDay;

        // Vérifier changement d'heure
        if (currentHour != _lastHour)
        {
            OnHourChanged?.Invoke(this, currentHour);
            _lastHour = currentHour;
        }

        // Vérifier changement de jour
        if (currentDay != _lastDay)
        {
            OnDayChanged?.Invoke(this, currentDay);
            _lastDay = currentDay;
        }

        // Vérifier changement de période
        if (currentTimeOfDay != _lastTimeOfDay)
        {
            OnTimeOfDayChanged?.Invoke(this, currentTimeOfDay);
            _lastTimeOfDay = currentTimeOfDay;
        }
    }

    /// <summary>
    /// Retourne une chaîne formatée de la date et heure actuelles IG.
    /// Format : "Jour X - HH:mm:ss"
    /// </summary>
    public string GetFormattedTime()
    {
        return $"Jour {CurrentDay} - {CurrentHour:D2}:{CurrentMinute:D2}:{CurrentSecond:D2}";
    }

    /// <summary>
    /// Retourne une chaîne formatée de la date complète IG.
    /// Format : "DD/MM/YYYY HH:mm:ss"
    /// </summary>
    public string GetFormattedDateTime()
    {
        DateTime time = CurrentGameTime;
        return time.ToString("dd/MM/yyyy HH:mm:ss");
    }

    /// <summary>
    /// Retourne le nom localisé de la période actuelle.
    /// </summary>
    public string GetTimeOfDayName()
    {
        return CurrentTimeOfDay switch
        {
            TimeOfDay.Night => "Nuit",
            TimeOfDay.Dawn => "Aube",
            TimeOfDay.Morning => "Matin",
            TimeOfDay.Afternoon => "Apres-midi",
            TimeOfDay.Dusk => "Crepuscule",
            TimeOfDay.Evening => "Soiree",
            _ => "Inconnu"
        };
    }

    // ========================================
    // MÉTHODES UTILITAIRES
    // ========================================

    /// <summary>
    /// Calcule combien de temps IRL correspond à une durée IG donnée.
    /// </summary>
    /// <param name="gameTimeSpan">Durée en temps IG</param>
    /// <returns>Durée équivalente en temps IRL</returns>
    public static TimeSpan ConvertGameTimeToRealTime(TimeSpan gameTimeSpan)
    {
        return TimeSpan.FromTicks((long)(gameTimeSpan.Ticks / TimeRatio));
    }

    /// <summary>
    /// Calcule combien de temps IG correspond à une durée IRL donnée.
    /// </summary>
    /// <param name="realTimeSpan">Durée en temps IRL</param>
    /// <returns>Durée équivalente en temps IG</returns>
    public static TimeSpan ConvertRealTimeToGameTime(TimeSpan realTimeSpan)
    {
        return TimeSpan.FromTicks((long)(realTimeSpan.Ticks * TimeRatio));
    }

    /// <summary>
    /// Retourne le nombre de secondes IRL pour 1 heure IG.
    /// </summary>
    public static double GetSecondsPerGameHour()
    {
        return 3600.0 / TimeRatio; // 3600 secondes / 1.2 = 3000 secondes = 50 minutes
    }

    /// <summary>
    /// Retourne le nombre de secondes IRL pour 1 jour IG.
    /// </summary>
    public static double GetSecondsPerGameDay()
    {
        return 86400.0 / TimeRatio; // 86400 secondes / 1.2 = 72000 secondes = 20 heures
    }
}
