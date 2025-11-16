using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Renderer pour afficher la légende des types de terrains.
/// </summary>
public class LegendRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private Texture2D? _pixelTexture;
    private SpriteFont? _font;

    private const int Padding = 10;
    private const int TileSize = 20;
    private const int LineHeight = 25;
    private const int LegendWidth = 300;

    public LegendRenderer(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        CreatePixelTexture();
    }

    /// <summary>
    /// Définit la font à utiliser pour le texte.
    /// </summary>
    public void SetFont(SpriteFont? font)
    {
        _font = font;
    }

    /// <summary>
    /// Crée une texture 1x1 pixel blanc pour le rendu.
    /// </summary>
    private void CreatePixelTexture()
    {
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    /// <summary>
    /// Dessine la légende des types de terrains.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (_pixelTexture == null)
            return;

        // Calculer la hauteur nécessaire pour la légende
        var terrainTypes = System.Enum.GetValues<TileType>();
        int legendHeight = Padding * 2 + 30 + (terrainTypes.Length * LineHeight);

        // Position de la légende (coin supérieur droit)
        int legendX = screenWidth - LegendWidth - Padding;
        int legendY = Padding;

        // Fond semi-transparent
        var backgroundRect = new Rectangle(legendX, legendY, LegendWidth, legendHeight);
        spriteBatch.Draw(_pixelTexture, backgroundRect, new Color(0, 0, 0, 200));

        // Bordure
        DrawRectangleOutline(spriteBatch, backgroundRect, Color.White, 2);

        int currentY = legendY + Padding;

        // Titre
        if (_font != null)
        {
            spriteBatch.DrawString(_font, "Légende des Terrains",
                new Vector2(legendX + Padding, currentY), Color.White);
        }
        currentY += 30;

        // Afficher chaque type de terrain
        foreach (TileType tileType in terrainTypes)
        {
            // Carré coloré représentant le terrain
            var tileRect = new Rectangle(legendX + Padding, currentY, TileSize, TileSize);
            var tileColor = TileColors.GetColor(tileType);
            spriteBatch.Draw(_pixelTexture, tileRect, tileColor);

            // Bordure du carré
            DrawRectangleOutline(spriteBatch, tileRect, Color.Gray, 1);

            // Nom du terrain
            if (_font != null)
            {
                string terrainName = GetTerrainName(tileType);
                spriteBatch.DrawString(_font, terrainName,
                    new Vector2(legendX + Padding + TileSize + 10, currentY + 2), Color.White);
            }

            currentY += LineHeight;
        }
    }

    /// <summary>
    /// Obtient le nom localisé d'un type de terrain.
    /// </summary>
    private string GetTerrainName(TileType type)
    {
        return type switch
        {
            TileType.DeepWater => "Eau Profonde",
            TileType.ShallowWater => "Eau Peu Profonde",
            TileType.Sand => "Sable",
            TileType.Grass => "Herbe",
            TileType.Dirt => "Terre",
            TileType.Forest => "Forêt Dense",
            TileType.SparseForest => "Forêt Clairsemée",
            TileType.Hill => "Colline",
            TileType.Mountain => "Montagne",
            TileType.SnowPeak => "Pic Enneigé",
            TileType.Swamp => "Marécage",
            TileType.Ruins => "Ruines",
            TileType.Toxic => "Zone Toxique",
            _ => "Inconnu"
        };
    }

    /// <summary>
    /// Dessine le contour d'un rectangle.
    /// </summary>
    private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int lineWidth)
    {
        if (_pixelTexture == null)
            return;

        // Top
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, lineWidth), color);
        // Bottom
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - lineWidth, rect.Width, lineWidth), color);
        // Left
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, lineWidth, rect.Height), color);
        // Right
        spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X + rect.Width - lineWidth, rect.Y, lineWidth, rect.Height), color);
    }

    public void Dispose()
    {
        _pixelTexture?.Dispose();
    }
}
