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
    /// Indique si le terrain réduit la visibilité (malus à la détection ennemie et à la visée).
    /// </summary>
    public bool HasReducedVisibility { get; init; }

    /// <summary>
    /// Indique si le terrain réduit la discrétion (malus à la furtivité, bruits de pas augmentés).
    /// </summary>
    public bool HasReducedStealth { get; init; }

    /// <summary>
    /// Indique si le terrain offre peu de couverture (malus pour se cacher, difficulté à se mettre à l'abri).
    /// </summary>
    public bool HasPoorCover { get; init; }

    /// <summary>
    /// Constructeur pour créer des propriétés de terrain.
    /// </summary>
    /// <param name="mobSpawnProbability">Probabilité de spawn de mobs (0.0 à 1.0)</param>
    /// <param name="resourceSpawnProbability">Probabilité de spawn de ressources (0.0 à 1.0)</param>
    /// <param name="itemSpawnProbability">Probabilité de spawn d'items (0.0 à 1.0)</param>
    /// <param name="isToxic">Terrain toxique (empoisonnement)</param>
    /// <param name="isDifficultTerrain">Terrain difficile (pas de course, plus de fatigue)</param>
    /// <param name="hasReducedVisibility">Visibilité réduite (malus détection/visée)</param>
    /// <param name="hasReducedStealth">Discrétion réduite (malus furtivité)</param>
    /// <param name="hasP oorCover">Peu de couverture (malus se cacher)</param>
    public TerrainProperties(
        float mobSpawnProbability,
        float resourceSpawnProbability,
        float itemSpawnProbability,
        bool isToxic = false,
        bool isDifficultTerrain = false,
        bool hasReducedVisibility = false,
        bool hasReducedStealth = false,
        bool hasPoorCover = false)
    {
        MobSpawnProbability = Math.Clamp(mobSpawnProbability, 0.0f, 1.0f);
        ResourceSpawnProbability = Math.Clamp(resourceSpawnProbability, 0.0f, 1.0f);
        ItemSpawnProbability = Math.Clamp(itemSpawnProbability, 0.0f, 1.0f);
        IsToxic = isToxic;
        IsDifficultTerrain = isDifficultTerrain;
        HasReducedVisibility = hasReducedVisibility;
        HasReducedStealth = hasReducedStealth;
        HasPoorCover = hasPoorCover;
    }

    /// <summary>
    /// Propriétés par défaut (toutes les probabilités à 0).
    /// </summary>
    public static TerrainProperties Default => new(0.0f, 0.0f, 0.0f, false, false, false, false, false);
}
