namespace TinySurvivalWorld.Core.World;

/// <summary>
/// Générateur de bruit de Perlin 2D simplifié pour la génération procédurale.
/// Basé sur l'algorithme de Ken Perlin.
/// </summary>
public class SimplexNoise
{
    private readonly int[] _permutation;
    private readonly Random _random;

    public SimplexNoise(long seed)
    {
        _random = new Random((int)seed);
        _permutation = new int[512];

        // Initialiser la table de permutation
        var p = new int[256];
        for (int i = 0; i < 256; i++)
        {
            p[i] = i;
        }

        // Mélanger aléatoirement
        for (int i = 255; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (p[i], p[j]) = (p[j], p[i]);
        }

        // Dupliquer pour éviter les wraparounds
        for (int i = 0; i < 512; i++)
        {
            _permutation[i] = p[i % 256];
        }
    }

    /// <summary>
    /// Génère une valeur de bruit 2D entre -1 et 1.
    /// </summary>
    public float Generate(float x, float y)
    {
        // Trouver la cellule de la grille
        int xi = (int)Math.Floor(x) & 255;
        int yi = (int)Math.Floor(y) & 255;

        // Coordonnées relatives dans la cellule (0-1)
        float xf = x - (float)Math.Floor(x);
        float yf = y - (float)Math.Floor(y);

        // Courbe de lissage (fade function)
        float u = Fade(xf);
        float v = Fade(yf);

        // Hash des coins de la cellule
        int aa = _permutation[_permutation[xi] + yi];
        int ab = _permutation[_permutation[xi] + yi + 1];
        int ba = _permutation[_permutation[xi + 1] + yi];
        int bb = _permutation[_permutation[xi + 1] + yi + 1];

        // Interpolation bilinéaire
        float x1 = Lerp(Gradient(aa, xf, yf), Gradient(ba, xf - 1, yf), u);
        float x2 = Lerp(Gradient(ab, xf, yf - 1), Gradient(bb, xf - 1, yf - 1), u);

        return Lerp(x1, x2, v);
    }

    /// <summary>
    /// Génère du bruit fractal (octaves multiples) entre -1 et 1.
    /// </summary>
    public float GenerateFractal(float x, float y, int octaves = 4, float persistence = 0.5f, float lacunarity = 2.0f)
    {
        float total = 0f;
        float frequency = 1f;
        float amplitude = 1f;
        float maxValue = 0f;

        for (int i = 0; i < octaves; i++)
        {
            total += Generate(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return total / maxValue;
    }

    /// <summary>
    /// Génère une valeur normalisée entre 0 et 1.
    /// </summary>
    public float GenerateNormalized(float x, float y, int octaves = 4, float persistence = 0.5f, float lacunarity = 2.0f)
    {
        return (GenerateFractal(x, y, octaves, persistence, lacunarity) + 1f) * 0.5f;
    }

    private static float Fade(float t)
    {
        // 6t^5 - 15t^4 + 10t^3
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private static float Gradient(int hash, float x, float y)
    {
        // Convertir les 4 bits de poids faible en 8 vecteurs de gradient
        int h = hash & 7;
        float u = h < 4 ? x : y;
        float v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
