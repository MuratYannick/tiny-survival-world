namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Configuration pour la génération procédurale du monde.
/// </summary>
public class WorldGenerationConfig
{
    // Elevation (altitude)
    public int ElevationOctaves { get; set; } = 4;
    public float ElevationPersistence { get; set; } = 0.5f;
    public float ElevationLacunarity { get; set; } = 2.0f;

    // Moisture (humidité)
    public int MoistureOctaves { get; set; } = 3;
    public float MoisturePersistence { get; set; } = 0.5f;
    public float MoistureLacunarity { get; set; } = 2.0f;

    // Temperature (température)
    public int TemperatureOctaves { get; set; } = 2;
    public float TemperaturePersistence { get; set; } = 0.5f;
    public float TemperatureLacunarity { get; set; } = 2.0f;

    /// <summary>
    /// Configuration par défaut.
    /// </summary>
    public static WorldGenerationConfig Default => new()
    {
        ElevationOctaves = 4,
        ElevationPersistence = 0.5f,
        ElevationLacunarity = 2.0f,

        MoistureOctaves = 3,
        MoisturePersistence = 0.5f,
        MoistureLacunarity = 2.0f,

        TemperatureOctaves = 2,
        TemperaturePersistence = 0.5f,
        TemperatureLacunarity = 2.0f
    };

    /// <summary>
    /// Crée une copie de la configuration.
    /// </summary>
    public WorldGenerationConfig Clone()
    {
        return new WorldGenerationConfig
        {
            ElevationOctaves = ElevationOctaves,
            ElevationPersistence = ElevationPersistence,
            ElevationLacunarity = ElevationLacunarity,

            MoistureOctaves = MoistureOctaves,
            MoisturePersistence = MoisturePersistence,
            MoistureLacunarity = MoistureLacunarity,

            TemperatureOctaves = TemperatureOctaves,
            TemperaturePersistence = TemperaturePersistence,
            TemperatureLacunarity = TemperatureLacunarity
        };
    }
}
