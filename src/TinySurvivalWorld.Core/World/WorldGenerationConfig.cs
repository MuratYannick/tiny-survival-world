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
    public float ElevationScale { get; set; } = 0.02f;  // Échelle du bruit (plus petit = biomes plus grands)
    public float ElevationOffset { get; set; } = 0.0f;  // Décalage des valeurs (-0.5 à +0.5)

    // Moisture (humidité)
    public int MoistureOctaves { get; set; } = 3;
    public float MoisturePersistence { get; set; } = 0.5f;
    public float MoistureLacunarity { get; set; } = 2.0f;
    public float MoistureScale { get; set; } = 0.03f;
    public float MoistureOffset { get; set; } = 0.0f;

    // Temperature (température)
    public int TemperatureOctaves { get; set; } = 2;
    public float TemperaturePersistence { get; set; } = 0.5f;
    public float TemperatureLacunarity { get; set; } = 2.0f;
    public float TemperatureScale { get; set; } = 0.025f;
    public float TemperatureOffset { get; set; } = 0.0f;

    /// <summary>
    /// Configuration par défaut.
    /// </summary>
    public static WorldGenerationConfig Default => new()
    {
        ElevationOctaves = 4,
        ElevationPersistence = 0.5f,
        ElevationLacunarity = 2.0f,
        ElevationScale = 0.02f,
        ElevationOffset = 0.0f,

        MoistureOctaves = 3,
        MoisturePersistence = 0.5f,
        MoistureLacunarity = 2.0f,
        MoistureScale = 0.03f,
        MoistureOffset = 0.0f,

        TemperatureOctaves = 2,
        TemperaturePersistence = 0.5f,
        TemperatureLacunarity = 2.0f,
        TemperatureScale = 0.025f,
        TemperatureOffset = 0.0f
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
            ElevationScale = ElevationScale,
            ElevationOffset = ElevationOffset,

            MoistureOctaves = MoistureOctaves,
            MoisturePersistence = MoisturePersistence,
            MoistureLacunarity = MoistureLacunarity,
            MoistureScale = MoistureScale,
            MoistureOffset = MoistureOffset,

            TemperatureOctaves = TemperatureOctaves,
            TemperaturePersistence = TemperaturePersistence,
            TemperatureLacunarity = TemperatureLacunarity,
            TemperatureScale = TemperatureScale,
            TemperatureOffset = TemperatureOffset
        };
    }
}
