using Microsoft.Xna.Framework;
using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Couleurs placeholder pour les différents types de tiles.
/// Seront remplacées par des sprites plus tard.
/// </summary>
public static class TileColors
{
    /// <summary>
    /// Obtient la couleur pour un type de tile donné.
    /// </summary>
    public static Color GetColor(TileType type)
    {
        return type switch
        {
            TileType.DeepWater => new Color(20, 50, 150),      // Bleu foncé
            TileType.ShallowWater => new Color(50, 100, 200),  // Bleu clair
            TileType.Sand => new Color(220, 200, 140),          // Beige/sable
            TileType.Grass => new Color(80, 150, 60),           // Vert prairie
            TileType.Dirt => new Color(140, 100, 60),           // Brun terre
            TileType.Forest => new Color(30, 90, 30),           // Vert forêt foncé
            TileType.SparseForest => new Color(60, 120, 50),    // Vert forêt clair
            TileType.Hill => new Color(120, 120, 90),           // Gris-vert colline
            TileType.Mountain => new Color(100, 100, 100),      // Gris montagne
            TileType.SnowPeak => new Color(240, 240, 250),      // Blanc neige
            TileType.Swamp => new Color(60, 80, 50),            // Vert-brun marécage
            TileType.Ruins => new Color(100, 80, 70),           // Gris-brun ruines
            TileType.Toxic => new Color(150, 180, 40),          // Vert-jaune toxique
            _ => Color.Magenta                                  // Erreur (rose criard)
        };
    }

    /// <summary>
    /// Obtient une variante légèrement différente de la couleur (pour variation visuelle).
    /// </summary>
    public static Color GetVariantColor(TileType type, int worldX, int worldY)
    {
        var baseColor = GetColor(type);

        // Utiliser les coordonnées pour créer une variation pseudo-aléatoire
        int hash = (worldX * 73856093) ^ (worldY * 19349663);
        int variation = (hash % 20) - 10; // Variation de -10 à +10

        return new Color(
            MathHelper.Clamp(baseColor.R + variation, 0, 255),
            MathHelper.Clamp(baseColor.G + variation, 0, 255),
            MathHelper.Clamp(baseColor.B + variation, 0, 255)
        );
    }
}
