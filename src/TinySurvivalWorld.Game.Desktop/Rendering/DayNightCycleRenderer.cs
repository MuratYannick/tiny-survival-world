using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TinySurvivalWorld.Core.Time;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Renderer pour le cycle jour/nuit avec variations saisonnières.
/// Applique un overlay de couleur basé sur l'intensité lumineuse du TimeManager.
/// </summary>
public class DayNightCycleRenderer : IDisposable
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly TimeManager _timeManager;
    private Texture2D? _overlayTexture;

    /// <summary>
    /// Constructeur du renderer de cycle jour/nuit.
    /// </summary>
    /// <param name="graphicsDevice">Device graphique MonoGame</param>
    /// <param name="timeManager">Gestionnaire du temps de jeu</param>
    public DayNightCycleRenderer(GraphicsDevice graphicsDevice, TimeManager timeManager)
    {
        _graphicsDevice = graphicsDevice;
        _timeManager = timeManager;

        // Créer une texture 1x1 pour l'overlay
        _overlayTexture = new Texture2D(_graphicsDevice, 1, 1);
        _overlayTexture.SetData(new[] { Color.White });
    }

    /// <summary>
    /// Dessine l'overlay de cycle jour/nuit sur tout l'écran.
    /// </summary>
    /// <param name="spriteBatch">SpriteBatch pour le rendu</param>
    /// <param name="screenWidth">Largeur de l'écran</param>
    /// <param name="screenHeight">Hauteur de l'écran</param>
    public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (_overlayTexture == null)
            return;

        // Récupérer l'intensité lumineuse actuelle (0.0 = nuit, 1.0 = jour)
        float lightIntensity = _timeManager.LightIntensity;

        // Créer une couleur d'overlay basée sur l'intensité
        // Pendant la nuit, on assombrit l'écran (teinte bleu foncé)
        // Pendant le jour, on laisse les couleurs normales

        // Couleur de base de la nuit (bleu très foncé)
        Color nightColor = new Color(10, 15, 30); // RGB: presque noir avec légère teinte bleue

        // Interpoler entre la couleur de nuit et le jour complet
        // lightIntensity = 0.15 (nuit) → nightColor fort
        // lightIntensity = 1.0 (jour) → transparent (pas d'overlay)

        // Normaliser l'intensité de 0.15-1.0 vers 0.0-1.0
        float normalizedIntensity = (lightIntensity - 0.15f) / (1.0f - 0.15f);
        normalizedIntensity = MathHelper.Clamp(normalizedIntensity, 0.0f, 1.0f);

        // Alpha de l'overlay : fort la nuit (0.7), nul le jour (0.0)
        float overlayAlpha = 1.0f - normalizedIntensity;
        overlayAlpha *= 0.7f; // Maximum 70% d'opacité pour ne pas être trop sombre

        // Appliquer la couleur d'overlay avec alpha
        Color overlayColor = nightColor * overlayAlpha;

        // Dessiner l'overlay en plein écran
        Rectangle screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
        spriteBatch.Draw(_overlayTexture, screenRect, overlayColor);
    }

    /// <summary>
    /// Libère les ressources.
    /// </summary>
    public void Dispose()
    {
        _overlayTexture?.Dispose();
        _overlayTexture = null;
    }
}
