namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Constantes pour le système de monde.
/// </summary>
public static class WorldConstants
{
    /// <summary>
    /// Taille d'un chunk en tiles (largeur et hauteur).
    /// </summary>
    public const int ChunkSize = 32;

    /// <summary>
    /// Taille d'une tile en pixels (pour le rendu).
    /// </summary>
    public const int TileSize = 32;

    /// <summary>
    /// Échelle pour le bruit de Perlin (élévation).
    /// Valeur plus grande = terrain plus varié.
    /// </summary>
    public const float ElevationScale = 0.02f;

    /// <summary>
    /// Échelle pour le bruit de Perlin (humidité).
    /// </summary>
    public const float MoistureScale = 0.03f;

    /// <summary>
    /// Échelle pour le bruit de Perlin (température).
    /// </summary>
    public const float TemperatureScale = 0.025f;

    /// <summary>
    /// Octaves pour le bruit fractal (plus = plus de détails).
    /// </summary>
    public const int NoiseOctaves = 4;

    /// <summary>
    /// Persistance du bruit fractal (influence des octaves supérieures).
    /// </summary>
    public const float NoisePersistence = 0.5f;

    /// <summary>
    /// Lacunarité du bruit fractal (fréquence entre octaves).
    /// </summary>
    public const float NoiseLacunarity = 2.0f;
}
