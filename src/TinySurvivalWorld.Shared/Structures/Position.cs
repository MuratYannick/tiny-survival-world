namespace TinySurvivalWorld.Shared.Structures;

/// <summary>
/// Représente une position 2D dans le monde du jeu.
/// Utilise une structure pour optimiser les performances (value type).
/// </summary>
public struct Position : IEquatable<Position>
{
    /// <summary>
    /// Coordonnée X (horizontale).
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// Coordonnée Y (verticale).
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Constructeur de position.
    /// </summary>
    /// <param name="x">Coordonnée X</param>
    /// <param name="y">Coordonnée Y</param>
    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Position à l'origine (0, 0).
    /// </summary>
    public static Position Zero => new(0f, 0f);

    /// <summary>
    /// Position unitaire (1, 1).
    /// </summary>
    public static Position One => new(1f, 1f);

    /// <summary>
    /// Calcule la distance euclidienne entre deux positions.
    /// </summary>
    /// <param name="a">Première position</param>
    /// <param name="b">Deuxième position</param>
    /// <returns>Distance entre les deux positions</returns>
    public static float Distance(Position a, Position b)
    {
        float dx = b.X - a.X;
        float dy = b.Y - a.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Calcule la distance Manhattan entre deux positions (|x1-x2| + |y1-y2|).
    /// Utile pour les grilles et le pathfinding.
    /// </summary>
    /// <param name="a">Première position</param>
    /// <param name="b">Deuxième position</param>
    /// <returns>Distance Manhattan</returns>
    public static float ManhattanDistance(Position a, Position b)
    {
        return MathF.Abs(b.X - a.X) + MathF.Abs(b.Y - a.Y);
    }

    /// <summary>
    /// Calcule la distance vers une autre position.
    /// </summary>
    /// <param name="other">Position cible</param>
    /// <returns>Distance euclidienne</returns>
    public float DistanceTo(Position other) => Distance(this, other);

    /// <summary>
    /// Interpole linéairement entre deux positions.
    /// </summary>
    /// <param name="a">Position de départ</param>
    /// <param name="b">Position d'arrivée</param>
    /// <param name="t">Facteur d'interpolation (0-1)</param>
    /// <returns>Position interpolée</returns>
    public static Position Lerp(Position a, Position b, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return new Position(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t
        );
    }

    // Opérateurs

    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    public static Position operator *(Position a, float scalar) => new(a.X * scalar, a.Y * scalar);
    public static Position operator /(Position a, float scalar) => new(a.X / scalar, a.Y / scalar);

    // Equality

    public bool Equals(Position other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Position other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public static bool operator ==(Position left, Position right) => left.Equals(right);
    public static bool operator !=(Position left, Position right) => !left.Equals(right);

    public override string ToString() => $"({X:F2}, {Y:F2})";
}
