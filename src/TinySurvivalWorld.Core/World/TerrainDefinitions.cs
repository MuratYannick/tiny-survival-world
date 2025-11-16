using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Définitions des propriétés pour chaque type de terrain.
/// </summary>
public static class TerrainDefinitions
{
    private static readonly Dictionary<TileType, TerrainProperties> _definitions = new()
    {
        // Eau profonde - Zone inaccessible, aucune apparition possible
        [TileType.DeepWater] = new TerrainProperties(
            mobSpawnProbability: 0.0f,      // Zone inaccessible
            resourceSpawnProbability: 0.0f, // Zone inaccessible
            itemSpawnProbability: 0.0f      // Zone inaccessible
        ),

        // Eau peu profonde - Zone côtière, quelques créatures aquatiques
        [TileType.ShallowWater] = new TerrainProperties(
            mobSpawnProbability: 0.2f,      // Quelques mobs aquatiques
            resourceSpawnProbability: 0.1f, // Quelques ressources (algues, coquillages)
            itemSpawnProbability: 0.05f     // Items très rares
        ),

        // Sable - Plage ou désert, peu de vie
        [TileType.Sand] = new TerrainProperties(
            mobSpawnProbability: 0.2f,      // Peu de mobs (scorpions, crabes)
            resourceSpawnProbability: 0.1f, // Peu de ressources (verre, sel)
            itemSpawnProbability: 0.1f      // Quelques items (débris)
        ),

        // Herbe - Prairie, zone de vie moyenne
        [TileType.Grass] = new TerrainProperties(
            mobSpawnProbability: 0.4f,      // Mobs moyens (animaux, créatures)
            resourceSpawnProbability: 0.3f, // Ressources moyennes (plantes, fibres)
            itemSpawnProbability: 0.2f      // Items moyens
        ),

        // Terre - Sol nu, moins fertile
        [TileType.Dirt] = new TerrainProperties(
            mobSpawnProbability: 0.3f,      // Mobs moyens
            resourceSpawnProbability: 0.2f, // Peu de ressources
            itemSpawnProbability: 0.1f      // Peu d'items
        ),

        // Forêt dense - Riche en ressources et vie sauvage
        [TileType.Forest] = new TerrainProperties(
            mobSpawnProbability: 0.6f,      // Beaucoup de mobs (animaux, créatures)
            resourceSpawnProbability: 0.8f, // Beaucoup de ressources (bois, plantes)
            itemSpawnProbability: 0.3f      // Items moyens
        ),

        // Forêt clairsemée - Moins dense, vie moyenne
        [TileType.SparseForest] = new TerrainProperties(
            mobSpawnProbability: 0.4f,      // Mobs moyens
            resourceSpawnProbability: 0.5f, // Ressources moyennes (bois)
            itemSpawnProbability: 0.2f      // Items moyens
        ),

        // Colline - Terrain élevé, quelques ressources minérales
        [TileType.Hill] = new TerrainProperties(
            mobSpawnProbability: 0.3f,      // Peu de mobs
            resourceSpawnProbability: 0.4f, // Ressources minérales moyennes
            itemSpawnProbability: 0.1f      // Peu d'items
        ),

        // Montagne - Riche en minerais, peu de vie
        [TileType.Mountain] = new TerrainProperties(
            mobSpawnProbability: 0.2f,      // Peu de mobs (créatures de montagne)
            resourceSpawnProbability: 0.7f, // Beaucoup de minerais
            itemSpawnProbability: 0.2f      // Quelques items (équipement abandonné)
        ),

        // Pic enneigé - Zone inaccessible, aucune apparition possible
        [TileType.SnowPeak] = new TerrainProperties(
            mobSpawnProbability: 0.0f,      // Zone inaccessible
            resourceSpawnProbability: 0.0f, // Zone inaccessible
            itemSpawnProbability: 0.0f      // Zone inaccessible
        ),

        // Marécage - Zone dangereuse, riche en vie hostile
        [TileType.Swamp] = new TerrainProperties(
            mobSpawnProbability: 0.7f,      // Beaucoup de mobs (créatures des marais)
            resourceSpawnProbability: 0.4f, // Ressources spéciales (plantes médicinales)
            itemSpawnProbability: 0.3f      // Items moyens
        ),

        // Ruines - Structures post-apocalyptiques, beaucoup d'items mais dangereux
        [TileType.Ruins] = new TerrainProperties(
            mobSpawnProbability: 0.8f,      // Beaucoup de mobs hostiles (mutants, raiders)
            resourceSpawnProbability: 0.2f, // Peu de ressources naturelles
            itemSpawnProbability: 0.7f      // Beaucoup d'items (butin, équipement)
        ),

        // Zone toxique - Très dangereux, mobs mutants, ressources contaminées
        [TileType.Toxic] = new TerrainProperties(
            mobSpawnProbability: 0.7f,      // Beaucoup de mobs mutants
            resourceSpawnProbability: 0.3f, // Ressources contaminées/rares
            itemSpawnProbability: 0.5f      // Items dangereux mais utiles
        )
    };

    /// <summary>
    /// Obtient les propriétés d'un type de terrain.
    /// </summary>
    /// <param name="tileType">Type de terrain</param>
    /// <returns>Propriétés du terrain, ou propriétés par défaut si non trouvé</returns>
    public static TerrainProperties GetProperties(TileType tileType)
    {
        return _definitions.TryGetValue(tileType, out var properties)
            ? properties
            : TerrainProperties.Default;
    }

    /// <summary>
    /// Vérifie si un type de terrain a des propriétés définies.
    /// </summary>
    public static bool HasProperties(TileType tileType)
    {
        return _definitions.ContainsKey(tileType);
    }

    /// <summary>
    /// Obtient tous les types de terrains définis avec leurs propriétés.
    /// </summary>
    public static IReadOnlyDictionary<TileType, TerrainProperties> GetAll()
    {
        return _definitions;
    }
}
