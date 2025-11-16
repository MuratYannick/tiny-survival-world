using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Générateur de monde procédural utilisant du bruit de Perlin.
/// </summary>
public class WorldGenerator
{
    private readonly SimplexNoise _elevationNoise;
    private readonly SimplexNoise _moistureNoise;
    private readonly SimplexNoise _temperatureNoise;
    private readonly long _seed;
    private readonly Random _random;
    private readonly WorldGenerationConfig _config;

    public WorldGenerator(long seed, WorldGenerationConfig? config = null)
    {
        _seed = seed;
        _random = new Random((int)seed);
        _config = config ?? WorldGenerationConfig.Default;

        // Créer 3 générateurs de bruit avec des seeds différentes
        _elevationNoise = new SimplexNoise(seed);
        _moistureNoise = new SimplexNoise(seed + 1000);
        _temperatureNoise = new SimplexNoise(seed + 2000);
    }

    /// <summary>
    /// Génère un chunk complet à la position spécifiée.
    /// </summary>
    public void GenerateChunk(Chunk chunk)
    {
        if (chunk.IsGenerated)
            return;

        var (startWorldX, startWorldY) = Chunk.ChunkToWorldCoords(chunk.ChunkX, chunk.ChunkY);

        for (int localX = 0; localX < WorldConstants.ChunkSize; localX++)
        {
            for (int localY = 0; localY < WorldConstants.ChunkSize; localY++)
            {
                int worldX = startWorldX + localX;
                int worldY = startWorldY + localY;

                var tile = GenerateTile(worldX, worldY);
                chunk.SetTile(localX, localY, tile);
            }
        }

        // Post-processing : ajouter des ruines aléatoires
        AddRandomRuins(chunk);

        chunk.IsGenerated = true;
    }

    /// <summary>
    /// Génère une tile individuelle aux coordonnées mondiales spécifiées.
    /// </summary>
    public Tile GenerateTile(int worldX, int worldY)
    {
        var tile = new Tile(worldX, worldY);

        // Générer l'élévation avec du bruit fractal + scale et offset configurables
        float elevationRaw = _elevationNoise.GenerateNormalized(
            worldX * _config.ElevationScale,
            worldY * _config.ElevationScale,
            _config.ElevationOctaves,
            _config.ElevationPersistence,
            _config.ElevationLacunarity
        );
        float elevation = Math.Clamp(elevationRaw + _config.ElevationOffset, 0f, 1f);

        // Générer l'humidité + scale et offset configurables
        float moistureRaw = _moistureNoise.GenerateNormalized(
            worldX * _config.MoistureScale,
            worldY * _config.MoistureScale,
            _config.MoistureOctaves,
            _config.MoisturePersistence,
            _config.MoistureLacunarity
        );
        float moisture = Math.Clamp(moistureRaw + _config.MoistureOffset, 0f, 1f);

        // Générer la température + scale et offset configurables (affectée par la latitude et l'élévation)
        float baseTempRaw = _temperatureNoise.GenerateNormalized(
            worldX * _config.TemperatureScale,
            worldY * _config.TemperatureScale,
            _config.TemperatureOctaves,
            _config.TemperaturePersistence,
            _config.TemperatureLacunarity
        );
        float baseTemp = Math.Clamp(baseTempRaw + _config.TemperatureOffset, 0f, 1f);

        // La température diminue avec l'élévation et la latitude
        float latitudeEffect = 1.0f - Math.Abs(worldY * 0.0001f); // Effet simplifié de latitude
        float elevationEffect = 1.0f - (elevation * 0.5f); // L'altitude refroidit
        float temperature = baseTemp * latitudeEffect * elevationEffect;

        tile.Elevation = elevation;
        tile.Moisture = moisture;
        tile.Temperature = temperature;

        // Déterminer le type de tile selon le biome
        tile.DetermineTypeFromBiome();

        return tile;
    }

    /// <summary>
    /// Ajoute aléatoirement des ruines dans le chunk (structures post-apocalyptiques).
    /// </summary>
    private void AddRandomRuins(Chunk chunk)
    {
        // Probabilité de ruines basée sur le seed du chunk
        int chunkSeed = HashChunkPosition(chunk.ChunkX, chunk.ChunkY);
        var chunkRandom = new Random(chunkSeed);

        // 5% de chance d'avoir des ruines dans un chunk
        if (chunkRandom.NextDouble() > 0.05)
            return;

        // Nombre de ruines (1 à 3)
        int ruinsCount = chunkRandom.Next(1, 4);

        for (int i = 0; i < ruinsCount; i++)
        {
            int localX = chunkRandom.Next(WorldConstants.ChunkSize);
            int localY = chunkRandom.Next(WorldConstants.ChunkSize);

            var tile = chunk.GetTile(localX, localY);
            if (tile != null && tile.IsWalkable)
            {
                // Convertir en ruines si la tile est traversable
                tile.Type = TileType.Ruins;
            }
        }
    }

    /// <summary>
    /// Hash une position de chunk pour obtenir un seed déterministe.
    /// </summary>
    private int HashChunkPosition(int chunkX, int chunkY)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + chunkX;
            hash = hash * 31 + chunkY;
            hash = hash * 31 + (int)_seed;
            return hash;
        }
    }

    /// <summary>
    /// Trouve une position de spawn valide (sur terre, traversable).
    /// </summary>
    public (float x, float y) FindValidSpawnPoint()
    {
        // Recherche en spirale autour de (0, 0)
        int searchRadius = 100;

        for (int radius = 0; radius < searchRadius; radius++)
        {
            for (int angle = 0; angle < 360; angle += 10)
            {
                float x = radius * MathF.Cos(angle * MathF.PI / 180f);
                float y = radius * MathF.Sin(angle * MathF.PI / 180f);

                var tile = GenerateTile((int)x, (int)y);

                if (tile.IsWalkable && tile.Type != TileType.ShallowWater)
                {
                    return (x, y);
                }
            }
        }

        // Par défaut, retourner (0, 0)
        return (0, 0);
    }
}
