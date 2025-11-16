using System.Collections.Concurrent;

namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Gestionnaire de chunks pour le streaming du monde.
/// Gère le chargement, déchargement et cache des chunks.
/// </summary>
public class ChunkManager
{
    private readonly ConcurrentDictionary<string, Chunk> _loadedChunks;
    private readonly WorldGenerator _worldGenerator;
    private readonly int _viewDistance;
    private readonly TimeSpan _unloadDelay;

    /// <summary>
    /// Nombre de chunks actuellement chargés en mémoire.
    /// </summary>
    public int LoadedChunkCount => _loadedChunks.Count;

    public ChunkManager(long worldSeed, int viewDistance = 3, int unloadDelayMinutes = 5)
    {
        _loadedChunks = new ConcurrentDictionary<string, Chunk>();
        _worldGenerator = new WorldGenerator(worldSeed);
        _viewDistance = viewDistance;
        _unloadDelay = TimeSpan.FromMinutes(unloadDelayMinutes);
    }

    /// <summary>
    /// Obtient un chunk à la position spécifiée, le génère si nécessaire.
    /// </summary>
    public Chunk GetOrCreateChunk(int chunkX, int chunkY)
    {
        string key = Chunk.GetChunkKey(chunkX, chunkY);

        return _loadedChunks.GetOrAdd(key, _ =>
        {
            var chunk = new Chunk(chunkX, chunkY);
            _worldGenerator.GenerateChunk(chunk);
            return chunk;
        });
    }

    /// <summary>
    /// Obtient une tile aux coordonnées mondiales spécifiées.
    /// </summary>
    public Tile? GetTile(int worldX, int worldY)
    {
        var (chunkX, chunkY) = Chunk.WorldToChunkCoords(worldX, worldY);
        var (localX, localY) = Chunk.WorldToLocalCoords(worldX, worldY);

        var chunk = GetOrCreateChunk(chunkX, chunkY);
        return chunk.GetTile(localX, localY);
    }

    /// <summary>
    /// Charge les chunks autour d'une position (caméra/joueur).
    /// </summary>
    public void LoadChunksAroundPosition(float worldX, float worldY)
    {
        var (centerChunkX, centerChunkY) = Chunk.WorldToChunkCoords((int)worldX, (int)worldY);

        // Charger les chunks dans le rayon de vision
        for (int x = -_viewDistance; x <= _viewDistance; x++)
        {
            for (int y = -_viewDistance; y <= _viewDistance; y++)
            {
                GetOrCreateChunk(centerChunkX + x, centerChunkY + y);
            }
        }
    }

    /// <summary>
    /// Décharge les chunks inactifs (non accédés depuis longtemps).
    /// </summary>
    public int UnloadInactiveChunks()
    {
        var now = DateTime.UtcNow;
        var chunksToUnload = new List<string>();

        foreach (var kvp in _loadedChunks)
        {
            if (now - kvp.Value.LastAccessed > _unloadDelay)
            {
                chunksToUnload.Add(kvp.Key);
            }
        }

        int unloadedCount = 0;
        foreach (var key in chunksToUnload)
        {
            if (_loadedChunks.TryRemove(key, out _))
            {
                unloadedCount++;
            }
        }

        return unloadedCount;
    }

    /// <summary>
    /// Décharge tous les chunks (pour libérer la mémoire).
    /// </summary>
    public void UnloadAllChunks()
    {
        _loadedChunks.Clear();
    }

    /// <summary>
    /// Obtient tous les chunks chargés dans une zone.
    /// </summary>
    public IEnumerable<Chunk> GetLoadedChunksInArea(int minChunkX, int minChunkY, int maxChunkX, int maxChunkY)
    {
        for (int x = minChunkX; x <= maxChunkX; x++)
        {
            for (int y = minChunkY; y <= maxChunkY; y++)
            {
                string key = Chunk.GetChunkKey(x, y);
                if (_loadedChunks.TryGetValue(key, out var chunk))
                {
                    yield return chunk;
                }
            }
        }
    }

    /// <summary>
    /// Précharge des chunks de manière asynchrone autour d'une position.
    /// </summary>
    public async Task PreloadChunksAsync(float worldX, float worldY, int radius = 5)
    {
        await Task.Run(() =>
        {
            var (centerChunkX, centerChunkY) = Chunk.WorldToChunkCoords((int)worldX, (int)worldY);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    GetOrCreateChunk(centerChunkX + x, centerChunkY + y);
                }
            }
        });
    }

    /// <summary>
    /// Trouve une position de spawn valide dans le monde.
    /// </summary>
    public (float x, float y) FindSpawnPoint()
    {
        return _worldGenerator.FindValidSpawnPoint();
    }
}
