using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Représente une tuile individuelle du monde.
/// </summary>
public class Tile
{
    /// <summary>
    /// Type de la tuile (terrain).
    /// </summary>
    public TileType Type { get; set; }

    /// <summary>
    /// Position X de la tuile dans le monde (coordonnées globales).
    /// </summary>
    public int WorldX { get; set; }

    /// <summary>
    /// Position Y de la tuile dans le monde (coordonnées globales).
    /// </summary>
    public int WorldY { get; set; }

    /// <summary>
    /// Élévation de la tuile (0.0 à 1.0).
    /// Utilisée pour la génération procédurale.
    /// </summary>
    public float Elevation { get; set; }

    /// <summary>
    /// Humidité de la tuile (0.0 à 1.0).
    /// Utilisée pour déterminer les biomes.
    /// </summary>
    public float Moisture { get; set; }

    /// <summary>
    /// Température de la tuile (-1.0 à 1.0).
    /// Affectée par l'élévation et la latitude.
    /// </summary>
    public float Temperature { get; set; }

    /// <summary>
    /// Propriétés du terrain (probabilités de spawn pour mobs, ressources, items).
    /// </summary>
    public TerrainProperties Properties => TerrainDefinitions.GetProperties(Type);

    /// <summary>
    /// Indique si la tuile est traversable par les personnages.
    /// </summary>
    public bool IsWalkable => Type switch
    {
        TileType.DeepWater => false,
        TileType.Mountain => false,
        TileType.SnowPeak => false,
        _ => true
    };

    /// <summary>
    /// Coût de déplacement sur cette tuile (1.0 = normal, >1.0 = ralentissement).
    /// </summary>
    public float MovementCost => Type switch
    {
        TileType.ShallowWater => 1.5f,
        TileType.Sand => 1.2f,
        TileType.Grass => 1.0f,
        TileType.Dirt => 1.0f,
        TileType.Forest => 1.3f,
        TileType.SparseForest => 1.1f,
        TileType.Hill => 1.4f,
        TileType.Swamp => 2.0f,
        TileType.Ruins => 1.2f,
        TileType.Toxic => 1.5f,
        _ => float.MaxValue // Non traversable
    };

    /// <summary>
    /// Indique si cette tuile peut contenir des ressources récoltables.
    /// </summary>
    public bool CanHaveResources => Type switch
    {
        TileType.Forest => true,
        TileType.SparseForest => true,
        TileType.Mountain => true,
        TileType.Hill => true,
        TileType.Ruins => true,
        _ => false
    };

    public Tile(int worldX, int worldY, TileType type = TileType.Grass)
    {
        WorldX = worldX;
        WorldY = worldY;
        Type = type;
        Elevation = 0.5f;
        Moisture = 0.5f;
        Temperature = 0.5f;
    }

    /// <summary>
    /// Détermine le type de tuile basé sur l'élévation, l'humidité et la température.
    /// </summary>
    public void DetermineTypeFromBiome()
    {
        // Eau (élévation basse)
        if (Elevation < 0.3f)
        {
            Type = Elevation < 0.15f ? TileType.DeepWater : TileType.ShallowWater;
            return;
        }

        // Montagne (élévation haute)
        if (Elevation > 0.8f)
        {
            Type = Elevation > 0.9f ? TileType.SnowPeak : TileType.Mountain;
            return;
        }

        // Collines
        if (Elevation > 0.65f)
        {
            Type = TileType.Hill;
            return;
        }

        // Plage/Sable (près de l'eau et sec)
        if (Elevation < 0.35f && Moisture < 0.3f)
        {
            Type = TileType.Sand;
            return;
        }

        // Marécage (bas et humide)
        if (Elevation < 0.4f && Moisture > 0.7f)
        {
            Type = TileType.Swamp;
            return;
        }

        // Forêt (humidité moyenne-haute, température modérée)
        if (Moisture > 0.5f && Temperature > 0.3f)
        {
            Type = Moisture > 0.7f ? TileType.Forest : TileType.SparseForest;
            return;
        }

        // Désert (sec)
        if (Moisture < 0.3f)
        {
            Type = TileType.Sand;
            return;
        }

        // Terre nue (transition)
        if (Moisture < 0.4f)
        {
            Type = TileType.Dirt;
            return;
        }

        // Par défaut : herbe
        Type = TileType.Grass;
    }
}
