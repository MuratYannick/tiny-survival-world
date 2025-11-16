using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TinySurvivalWorld.Core.World;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Renderer pour les tiles du monde.
/// </summary>
public class TileRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly ChunkManager _chunkManager;
    private Texture2D? _pixelTexture;

    /// <summary>
    /// Nombre de tiles rendues lors du dernier frame (debug).
    /// </summary>
    public int TilesRenderedLastFrame { get; private set; }

    /// <summary>
    /// Nombre de chunks rendus lors du dernier frame (debug).
    /// </summary>
    public int ChunksRenderedLastFrame { get; private set; }

    public TileRenderer(GraphicsDevice graphicsDevice, ChunkManager chunkManager)
    {
        _graphicsDevice = graphicsDevice;
        _chunkManager = chunkManager;
        CreatePixelTexture();
    }

    /// <summary>
    /// Crée une texture 1x1 pixel blanc pour le rendu des rectangles colorés.
    /// </summary>
    private void CreatePixelTexture()
    {
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    /// <summary>
    /// Rend les tiles visibles dans la zone de la caméra.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        if (_pixelTexture == null)
            return;

        TilesRenderedLastFrame = 0;
        ChunksRenderedLastFrame = 0;

        // Obtenir la zone visible de la caméra (en pixels)
        var visibleArea = camera.GetVisibleArea();

        // Convertir en coordonnées de tiles
        int minTileX = visibleArea.Left / WorldConstants.TileSize - 1;
        int minTileY = visibleArea.Top / WorldConstants.TileSize - 1;
        int maxTileX = (visibleArea.Right / WorldConstants.TileSize) + 1;
        int maxTileY = (visibleArea.Bottom / WorldConstants.TileSize) + 1;

        // Convertir en coordonnées de chunks
        var (minChunkX, minChunkY) = Chunk.WorldToChunkCoords(minTileX, minTileY);
        var (maxChunkX, maxChunkY) = Chunk.WorldToChunkCoords(maxTileX, maxTileY);

        // Précharger les chunks visibles
        for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
        {
            for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
            {
                _chunkManager.GetOrCreateChunk(chunkX, chunkY);
            }
        }

        // Rendre les chunks
        for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
        {
            for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
            {
                DrawChunk(spriteBatch, chunkX, chunkY);
                ChunksRenderedLastFrame++;
            }
        }
    }

    /// <summary>
    /// Rend un chunk spécifique.
    /// </summary>
    private void DrawChunk(SpriteBatch spriteBatch, int chunkX, int chunkY)
    {
        var chunk = _chunkManager.GetOrCreateChunk(chunkX, chunkY);

        if (!chunk.IsGenerated)
            return;

        for (int localX = 0; localX < WorldConstants.ChunkSize; localX++)
        {
            for (int localY = 0; localY < WorldConstants.ChunkSize; localY++)
            {
                var tile = chunk.GetTile(localX, localY);
                if (tile != null)
                {
                    DrawTile(spriteBatch, tile);
                    TilesRenderedLastFrame++;
                }
            }
        }
    }

    /// <summary>
    /// Rend une tile individuelle.
    /// </summary>
    private void DrawTile(SpriteBatch spriteBatch, Tile tile)
    {
        if (_pixelTexture == null)
            return;

        // Position en pixels dans le monde
        var position = new Vector2(
            tile.WorldX * WorldConstants.TileSize,
            tile.WorldY * WorldConstants.TileSize
        );

        // Rectangle de la tile
        var rect = new Rectangle(
            (int)position.X,
            (int)position.Y,
            WorldConstants.TileSize,
            WorldConstants.TileSize
        );

        // Couleur avec variation
        var color = TileColors.GetVariantColor(tile.Type, tile.WorldX, tile.WorldY);

        // Dessiner la tile
        spriteBatch.Draw(
            _pixelTexture,
            rect,
            color
        );
    }

    /// <summary>
    /// Dessine une grille de chunks pour le debug.
    /// </summary>
    public void DrawChunkGrid(SpriteBatch spriteBatch, Camera2D camera)
    {
        if (_pixelTexture == null)
            return;

        var visibleArea = camera.GetVisibleArea();

        int minTileX = visibleArea.Left / WorldConstants.TileSize - 1;
        int minTileY = visibleArea.Top / WorldConstants.TileSize - 1;
        int maxTileX = (visibleArea.Right / WorldConstants.TileSize) + 1;
        int maxTileY = (visibleArea.Bottom / WorldConstants.TileSize) + 1;

        var (minChunkX, minChunkY) = Chunk.WorldToChunkCoords(minTileX, minTileY);
        var (maxChunkX, maxChunkY) = Chunk.WorldToChunkCoords(maxTileX, maxTileY);

        for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
        {
            for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
            {
                var (worldX, worldY) = Chunk.ChunkToWorldCoords(chunkX, chunkY);
                int pixelX = worldX * WorldConstants.TileSize;
                int pixelY = worldY * WorldConstants.TileSize;
                int chunkPixelSize = WorldConstants.ChunkSize * WorldConstants.TileSize;

                // Dessiner le contour du chunk
                DrawRectangleOutline(spriteBatch, pixelX, pixelY, chunkPixelSize, chunkPixelSize, Color.Yellow);
            }
        }
    }

    /// <summary>
    /// Dessine le contour d'un rectangle (helper pour debug).
    /// </summary>
    private void DrawRectangleOutline(SpriteBatch spriteBatch, int x, int y, int width, int height, Color color)
    {
        if (_pixelTexture == null)
            return;

        int lineWidth = 2;

        // Top
        spriteBatch.Draw(_pixelTexture, new Rectangle(x, y, width, lineWidth), color);
        // Bottom
        spriteBatch.Draw(_pixelTexture, new Rectangle(x, y + height - lineWidth, width, lineWidth), color);
        // Left
        spriteBatch.Draw(_pixelTexture, new Rectangle(x, y, lineWidth, height), color);
        // Right
        spriteBatch.Draw(_pixelTexture, new Rectangle(x + width - lineWidth, y, lineWidth, height), color);
    }

    public void Dispose()
    {
        _pixelTexture?.Dispose();
    }
}
