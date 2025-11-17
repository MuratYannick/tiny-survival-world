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

    /// <summary>
    /// Jour de l'année (1-365) dans le calendrier IG.
    /// </summary>
    public int DayOfYear => CurrentGameTime.DayOfYear;

    /// <summary>
    /// Heure du lever du soleil (début de l'aube) selon la saison (varie de 5h à 7h).
    /// </summary>
    public float SunriseHour => CalculateSunriseHour();

    /// <summary>
    /// Heure du coucher du soleil (début du crépuscule) selon la saison (varie de 17h à 19h).
    /// </summary>
    public float SunsetHour => CalculateSunsetHour();

    /// <summary>
    /// Durée totale de lumière (aube + jour + crépuscule) en heures.
    /// Varie de 10h en hiver à 18h en été.
    /// </summary>
    public float DaylightDuration => CalculateDaylightDuration();

    /// <summary>
    /// Intensité lumineuse actuelle (0.0 = nuit noire, 1.0 = plein jour).
    /// Transitions douces à l'aube et au crépuscule.
    /// </summary>
    public float LightIntensity => CalculateLightIntensity();

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

    // ========================================
    // CALCULS CYCLE JOUR/NUIT SAISONNIER
    // ========================================

    /// <summary>
    /// Calcule la durée de lumière (heures) selon le jour de l'année.
    /// Solstice d'hiver (21 déc, jour ~355) : 10h de lumière
    /// Solstice d'été (21 juin, jour ~172) : 18h de lumière
    /// Variation sinusoïdale entre les deux.
    /// </summary>
    private float CalculateDaylightDuration()
    {
        // Constantes de variation saisonnière
        const float minDaylight = 10.0f; // Hiver : 10h de lumière (8h jour + 2h transitions)
        const float maxDaylight = 18.0f; // Été : 18h de lumière (16h jour + 2h transitions)
        const int summerSolstice = 172;  // ~21 juin (jour le plus long)

        // Jour de l'année (1-365)
        int dayOfYear = DayOfYear;

        // Décalage pour que le pic soit au solstice d'été
        // sin atteint son maximum à π/2, donc on décale de summerSolstice jours
        float angle = (dayOfYear - summerSolstice) * 2.0f * MathF.PI / 365.0f;

        // Variation sinusoïdale : -1 (hiver) à +1 (été)
        float seasonalVariation = MathF.Sin(angle);

        // Interpolation entre min et max
        float avgDaylight = (minDaylight + maxDaylight) / 2.0f; // 14h
        float amplitude = (maxDaylight - minDaylight) / 2.0f;    // 4h

        return avgDaylight + (seasonalVariation * amplitude);
    }

    /// <summary>
    /// Calcule l'heure du lever du soleil (début de l'aube) selon la saison.
    /// Centre le jour autour de 12h (midi).
    /// </summary>
    private float CalculateSunriseHour()
    {
        float daylightDuration = DaylightDuration;
        // Le jour est centré sur 12h, donc lever = 12 - (durée/2)
        return 12.0f - (daylightDuration / 2.0f);
    }

    /// <summary>
    /// Calcule l'heure du coucher du soleil (début du crépuscule) selon la saison.
    /// </summary>
    private float CalculateSunsetHour()
    {
        float daylightDuration = DaylightDuration;
        // Coucher = lever + durée de lumière
        return SunriseHour + daylightDuration;
    }

    /// <summary>
    /// Calcule l'intensité lumineuse actuelle (0.0 = nuit, 1.0 = jour).
    /// Transitions douces pendant l'aube (1h) et le crépuscule (1h).
    /// </summary>
    private float CalculateLightIntensity()
    {
        float currentHour = CurrentHour + (CurrentMinute / 60.0f); // Heure décimale
        float sunrise = SunriseHour;
        float sunset = SunsetHour;

        const float transitionDuration = 1.0f; // 1 heure pour aube et crépuscule

        // Nuit profonde (avant l'aube)
        if (currentHour < sunrise)
        {
            return 0.15f; // Nuit = 15% de lumière (pour ne pas être totalement noir)
        }
        // Aube (transition 1h)
        else if (currentHour < sunrise + transitionDuration)
        {
            float progress = (currentHour - sunrise) / transitionDuration;
            return 0.15f + (1.0f - 0.15f) * progress; // Transition douce 15% → 100%
        }
        // Jour (pleine lumière)
        else if (currentHour < sunset)
        {
            return 1.0f; // Plein jour = 100%
        }
        // Crépuscule (transition 1h)
        else if (currentHour < sunset + transitionDuration)
        {
            float progress = (currentHour - sunset) / transitionDuration;
            return 1.0f + (0.15f - 1.0f) * progress; // Transition douce 100% → 15%
        }
        // Nuit (après le crépuscule)
        else
        {
            return 0.15f; // Nuit = 15%
        }
    }
}

