namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Représente un chunk du monde (section carrée de tiles).
/// </summary>
public class Chunk
{
    /// <summary>
    /// Position X du chunk dans la grille de chunks.
    /// </summary>
    public int ChunkX { get; }

    /// <summary>
    /// Position Y du chunk dans la grille de chunks.
    /// </summary>
    public int ChunkY { get; }

    /// <summary>
    /// Grille de tiles du chunk [x, y].
    /// </summary>
    public Tile[,] Tiles { get; }

    /// <summary>
    /// Indique si le chunk a été généré.
    /// </summary>
    public bool IsGenerated { get; set; }

    /// <summary>
    /// Timestamp de la dernière utilisation (pour unload des chunks inactifs).
    /// </summary>
    public DateTime LastAccessed { get; set; }

    public Chunk(int chunkX, int chunkY)
    {
        ChunkX = chunkX;
        ChunkY = chunkY;
        Tiles = new Tile[WorldConstants.ChunkSize, WorldConstants.ChunkSize];
        IsGenerated = false;
        LastAccessed = DateTime.UtcNow;
    }

    /// <summary>
    /// Obtient une tile aux coordonnées locales du chunk (0 à ChunkSize-1).
    /// </summary>
    public Tile? GetTile(int localX, int localY)
    {
        if (localX < 0 || localX >= WorldConstants.ChunkSize ||
            localY < 0 || localY >= WorldConstants.ChunkSize)
        {
            return null;
        }

        LastAccessed = DateTime.UtcNow;
        return Tiles[localX, localY];
    }

    /// <summary>
    /// Définit une tile aux coordonnées locales du chunk.
    /// </summary>
    public void SetTile(int localX, int localY, Tile tile)
    {
        if (localX < 0 || localX >= WorldConstants.ChunkSize ||
            localY < 0 || localY >= WorldConstants.ChunkSize)
        {
            return;
        }

        Tiles[localX, localY] = tile;
        LastAccessed = DateTime.UtcNow;
    }

    /// <summary>
    /// Convertit des coordonnées mondiales en coordonnées de chunk.
    /// </summary>
    public static (int chunkX, int chunkY) WorldToChunkCoords(int worldX, int worldY)
    {
        int chunkX = worldX >= 0
            ? worldX / WorldConstants.ChunkSize
            : (worldX + 1) / WorldConstants.ChunkSize - 1;

        int chunkY = worldY >= 0
            ? worldY / WorldConstants.ChunkSize
            : (worldY + 1) / WorldConstants.ChunkSize - 1;

        return (chunkX, chunkY);
    }

    /// <summary>
    /// Convertit des coordonnées mondiales en coordonnées locales dans le chunk.
    /// </summary>
    public static (int localX, int localY) WorldToLocalCoords(int worldX, int worldY)
    {
        int localX = worldX >= 0
            ? worldX % WorldConstants.ChunkSize
            : WorldConstants.ChunkSize - 1 - ((-worldX - 1) % WorldConstants.ChunkSize);

        int localY = worldY >= 0
            ? worldY % WorldConstants.ChunkSize
            : WorldConstants.ChunkSize - 1 - ((-worldY - 1) % WorldConstants.ChunkSize);

        return (localX, localY);
    }

    /// <summary>
    /// Convertit des coordonnées de chunk en coordonnées mondiales (coin supérieur gauche).
    /// </summary>
    public static (int worldX, int worldY) ChunkToWorldCoords(int chunkX, int chunkY)
    {
        return (chunkX * WorldConstants.ChunkSize, chunkY * WorldConstants.ChunkSize);
    }

    /// <summary>
    /// Calcule la clé unique pour ce chunk (pour dictionnaire).
    /// </summary>
    public string GetChunkKey() => $"{ChunkX},{ChunkY}";

    /// <summary>
    /// Calcule la clé unique pour un chunk depuis des coordonnées.
    /// </summary>
    public static string GetChunkKey(int chunkX, int chunkY) => $"{chunkX},{chunkY}";
}
