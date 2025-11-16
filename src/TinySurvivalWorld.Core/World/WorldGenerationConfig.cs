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
    public float ElevationScale { get; set; } = 1.0f;   // Compression/extension des valeurs (1.0 = normal)
    public float ElevationOffset { get; set; } = 0.5f;  // Centre de l'échelle (0.5 = centré)

    // Moisture (humidité)
    public int MoistureOctaves { get; set; } = 3;
    public float MoisturePersistence { get; set; } = 0.5f;
    public float MoistureLacunarity { get; set; } = 2.0f;
    public float MoistureScale { get; set; } = 1.0f;
    public float MoistureOffset { get; set; } = 0.5f;

    // Temperature (température)
    public int TemperatureOctaves { get; set; } = 2;
    public float TemperaturePersistence { get; set; } = 0.5f;
    public float TemperatureLacunarity { get; set; } = 2.0f;
    public float TemperatureScale { get; set; } = 1.0f;
    public float TemperatureOffset { get; set; } = 0.5f;

    /// <summary>
    /// Configuration par défaut.
    /// </summary>
    public static WorldGenerationConfig Default => new()
    {
        ElevationOctaves = 4,
        ElevationPersistence = 0.5f,
        ElevationLacunarity = 2.0f,
        ElevationScale = 1.0f,
        ElevationOffset = 0.5f,

        MoistureOctaves = 3,
        MoisturePersistence = 0.5f,
        MoistureLacunarity = 2.0f,
        MoistureScale = 1.0f,
        MoistureOffset = 0.5f,

        TemperatureOctaves = 2,
        TemperaturePersistence = 0.5f,
        TemperatureLacunarity = 2.0f,
        TemperatureScale = 1.0f,
        TemperatureOffset = 0.5f
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
