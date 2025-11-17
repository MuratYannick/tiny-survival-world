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
    /// Rend une tile individuelle avec blending vers les tiles voisines.
    /// </summary>
    private void DrawTile(SpriteBatch spriteBatch, Tile tile)
    {
        if (_pixelTexture == null)
            return;

        // Position en pixels dans le monde
        int pixelX = tile.WorldX * WorldConstants.TileSize;
        int pixelY = tile.WorldY * WorldConstants.TileSize;

        // Couleur de base avec variation
        var baseColor = TileColors.GetVariantColor(tile.Type, tile.WorldX, tile.WorldY);

        // Récupérer les tiles voisines (N, S, E, W)
        var northTile = GetTileAt(tile.WorldX, tile.WorldY - 1);
        var southTile = GetTileAt(tile.WorldX, tile.WorldY + 1);
        var eastTile = GetTileAt(tile.WorldX + 1, tile.WorldY);
        var westTile = GetTileAt(tile.WorldX - 1, tile.WorldY);

        // Taille de la bordure de transition (en pixels)
        int blendSize = WorldConstants.TileSize / 4; // 25% de la tile

        // Dessiner le centre de la tile (couleur pure)
        var centerRect = new Rectangle(
            pixelX + blendSize,
            pixelY + blendSize,
            WorldConstants.TileSize - blendSize * 2,
            WorldConstants.TileSize - blendSize * 2
        );
        spriteBatch.Draw(_pixelTexture, centerRect, baseColor);

        // Dessiner les bordures avec blending si le voisin est différent
        DrawBorder(spriteBatch, pixelX, pixelY, tile, northTile, baseColor, BorderSide.North, blendSize);
        DrawBorder(spriteBatch, pixelX, pixelY, tile, southTile, baseColor, BorderSide.South, blendSize);
        DrawBorder(spriteBatch, pixelX, pixelY, tile, eastTile, baseColor, BorderSide.East, blendSize);
        DrawBorder(spriteBatch, pixelX, pixelY, tile, westTile, baseColor, BorderSide.West, blendSize);

        // Dessiner les coins
        DrawCorner(spriteBatch, pixelX, pixelY, tile, northTile, westTile, baseColor, CornerPosition.NorthWest, blendSize);
        DrawCorner(spriteBatch, pixelX, pixelY, tile, northTile, eastTile, baseColor, CornerPosition.NorthEast, blendSize);
        DrawCorner(spriteBatch, pixelX, pixelY, tile, southTile, westTile, baseColor, CornerPosition.SouthWest, blendSize);
        DrawCorner(spriteBatch, pixelX, pixelY, tile, southTile, eastTile, baseColor, CornerPosition.SouthEast, blendSize);
    }

    /// <summary>
    /// Récupère une tile à une position donnée (peut retourner null).
    /// </summary>
    private Tile? GetTileAt(int worldX, int worldY)
    {
        var (chunkX, chunkY) = Chunk.WorldToChunkCoords(worldX, worldY);
        var chunk = _chunkManager.GetOrCreateChunk(chunkX, chunkY);

        if (!chunk.IsGenerated)
            return null;

        var (localX, localY) = Chunk.WorldToLocalCoords(worldX, worldY);
        return chunk.GetTile(localX, localY);
    }

    private enum BorderSide { North, South, East, West }
    private enum CornerPosition { NorthWest, NorthEast, SouthWest, SouthEast }

    /// <summary>
    /// Dessine une bordure avec blending vers le voisin si différent.
    /// </summary>
    private void DrawBorder(SpriteBatch spriteBatch, int pixelX, int pixelY, Tile tile, Tile? neighbor,
        Color baseColor, BorderSide side, int blendSize)
    {
        if (_pixelTexture == null)
            return;

        // Si pas de voisin ou voisin identique, dessiner la couleur de base
        if (neighbor == null || neighbor.Type == tile.Type)
        {
            Rectangle rect = side switch
            {
                BorderSide.North => new Rectangle(pixelX + blendSize, pixelY, WorldConstants.TileSize - blendSize * 2, blendSize),
                BorderSide.South => new Rectangle(pixelX + blendSize, pixelY + WorldConstants.TileSize - blendSize, WorldConstants.TileSize - blendSize * 2, blendSize),
                BorderSide.East => new Rectangle(pixelX + WorldConstants.TileSize - blendSize, pixelY + blendSize, blendSize, WorldConstants.TileSize - blendSize * 2),
                BorderSide.West => new Rectangle(pixelX, pixelY + blendSize, blendSize, WorldConstants.TileSize - blendSize * 2),
                _ => Rectangle.Empty
            };

            if (rect != Rectangle.Empty)
                spriteBatch.Draw(_pixelTexture, rect, baseColor);
            return;
        }

        // Voisin différent : faire un gradient
        var neighborColor = TileColors.GetColor(neighbor.Type);
        int steps = 4; // Nombre de bandes de gradient

        for (int i = 0; i < steps; i++)
        {
            float blend = (float)i / steps;
            var gradientColor = TileColors.Lerp(neighborColor, baseColor, blend);

            Rectangle rect = side switch
            {
                BorderSide.North => new Rectangle(
                    pixelX + blendSize,
                    pixelY + (i * blendSize / steps),
                    WorldConstants.TileSize - blendSize * 2,
                    blendSize / steps),
                BorderSide.South => new Rectangle(
                    pixelX + blendSize,
                    pixelY + WorldConstants.TileSize - blendSize + (i * blendSize / steps),
                    WorldConstants.TileSize - blendSize * 2,
                    blendSize / steps),
                BorderSide.East => new Rectangle(
                    pixelX + WorldConstants.TileSize - blendSize + (i * blendSize / steps),
                    pixelY + blendSize,
                    blendSize / steps,
                    WorldConstants.TileSize - blendSize * 2),
                BorderSide.West => new Rectangle(
                    pixelX + (i * blendSize / steps),
                    pixelY + blendSize,
                    blendSize / steps,
                    WorldConstants.TileSize - blendSize * 2),
                _ => Rectangle.Empty
            };

            if (rect != Rectangle.Empty)
                spriteBatch.Draw(_pixelTexture, rect, gradientColor);
        }
    }

    /// <summary>
    /// Dessine un coin avec blending vers les voisins si différents.
    /// </summary>
    private void DrawCorner(SpriteBatch spriteBatch, int pixelX, int pixelY, Tile tile,
        Tile? neighborV, Tile? neighborH, Color baseColor, CornerPosition corner, int blendSize)
    {
        if (_pixelTexture == null)
            return;

        // Position du coin
        Rectangle rect = corner switch
        {
            CornerPosition.NorthWest => new Rectangle(pixelX, pixelY, blendSize, blendSize),
            CornerPosition.NorthEast => new Rectangle(pixelX + WorldConstants.TileSize - blendSize, pixelY, blendSize, blendSize),
            CornerPosition.SouthWest => new Rectangle(pixelX, pixelY + WorldConstants.TileSize - blendSize, blendSize, blendSize),
            CornerPosition.SouthEast => new Rectangle(pixelX + WorldConstants.TileSize - blendSize, pixelY + WorldConstants.TileSize - blendSize, blendSize, blendSize),
            _ => Rectangle.Empty
        };

        if (rect == Rectangle.Empty)
            return;

        // Déterminer la couleur du coin en fonction des voisins
        Color cornerColor = baseColor;

        // Si les deux voisins sont identiques et différents de la tile actuelle
        if (neighborV != null && neighborH != null &&
            neighborV.Type == neighborH.Type && neighborV.Type != tile.Type)
        {
            // Blend vers la couleur commune des voisins
            var neighborColor = TileColors.GetColor(neighborV.Type);
            cornerColor = TileColors.Lerp(baseColor, neighborColor, 0.7f);
        }
        // Si un seul voisin est différent, blend vers lui
        else if (neighborV != null && neighborV.Type != tile.Type)
        {
            var neighborColor = TileColors.GetColor(neighborV.Type);
            cornerColor = TileColors.Lerp(baseColor, neighborColor, 0.5f);
        }
        else if (neighborH != null && neighborH.Type != tile.Type)
        {
            var neighborColor = TileColors.GetColor(neighborH.Type);
            cornerColor = TileColors.Lerp(baseColor, neighborColor, 0.5f);
        }

        spriteBatch.Draw(_pixelTexture, rect, cornerColor);
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
