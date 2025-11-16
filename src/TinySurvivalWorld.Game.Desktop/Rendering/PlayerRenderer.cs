using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TinySurvivalWorld.Game.Desktop.Entities;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Renderer pour le personnage joueur.
/// </summary>
public class PlayerRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private Texture2D? _pixelTexture;

    public PlayerRenderer(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        CreatePixelTexture();
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
    /// Dessine le personnage joueur.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, PlayerCharacter player)
    {
        if (_pixelTexture == null)
            return;

        // Dessiner le corps du personnage (rectangle bleu)
        var bodyRect = new Rectangle(
            (int)(player.Position.X - player.Size / 2),
            (int)(player.Position.Y - player.Size / 2),
            player.Size,
            player.Size
        );

        spriteBatch.Draw(_pixelTexture, bodyRect, new Color(50, 150, 255)); // Bleu joueur

        // Dessiner une bordure pour mieux le voir
        DrawRectangleOutline(spriteBatch, bodyRect, Color.White, 2);

        // Dessiner un point central pour indiquer la position exacte
        var centerRect = new Rectangle(
            (int)player.Position.X - 2,
            (int)player.Position.Y - 2,
            4,
            4
        );
        spriteBatch.Draw(_pixelTexture, centerRect, Color.Yellow);
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
