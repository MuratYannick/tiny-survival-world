namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Propriétés d'un type de terrain (biome).
/// </summary>
public class TerrainProperties
{
    /// <summary>
    /// Probabilité de rencontre avec un mob (0.0 = jamais, 1.0 = très élevée).
    /// </summary>
    public float MobSpawnProbability { get; init; }

    /// <summary>
    /// Probabilité de trouver des ressources extractibles (0.0 = jamais, 1.0 = très élevée).
    /// </summary>
    public float ResourceSpawnProbability { get; init; }

    /// <summary>
    /// Probabilité de trouver des items au sol (0.0 = jamais, 1.0 = très élevée).
    /// </summary>
    public float ItemSpawnProbability { get; init; }

    /// <summary>
    /// Indique si le terrain est toxique (empoisonnement progressif au fil du temps).
    /// </summary>
    public bool IsToxic { get; init; }

    /// <summary>
    /// Indique si le terrain est difficile (empêche de courir, augmente fatigue).
    /// </summary>
    public bool IsDifficultTerrain { get; init; }

    /// <summary>
    /// Constructeur pour créer des propriétés de terrain.
    /// </summary>
    /// <param name="mobSpawnProbability">Probabilité de spawn de mobs (0.0 à 1.0)</param>
    /// <param name="resourceSpawnProbability">Probabilité de spawn de ressources (0.0 à 1.0)</param>
    /// <param name="itemSpawnProbability">Probabilité de spawn d'items (0.0 à 1.0)</param>
    /// <param name="isToxic">Terrain toxique (empoisonnement)</param>
    /// <param name="isDifficultTerrain">Terrain difficile (pas de course, plus de fatigue)</param>
    public TerrainProperties(
        float mobSpawnProbability,
        float resourceSpawnProbability,
        float itemSpawnProbability,
        bool isToxic = false,
        bool isDifficultTerrain = false)
    {
        MobSpawnProbability = Math.Clamp(mobSpawnProbability, 0.0f, 1.0f);
        ResourceSpawnProbability = Math.Clamp(resourceSpawnProbability, 0.0f, 1.0f);
        ItemSpawnProbability = Math.Clamp(itemSpawnProbability, 0.0f, 1.0f);
        IsToxic = isToxic;
        IsDifficultTerrain = isDifficultTerrain;
    }

    /// <summary>
    /// Propriétés par défaut (toutes les probabilités à 0).
    /// </summary>
    public static TerrainProperties Default => new(0.0f, 0.0f, 0.0f, false, false);
}
