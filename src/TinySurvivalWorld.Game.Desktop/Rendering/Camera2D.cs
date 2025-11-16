using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TinySurvivalWorld.Game.Desktop.Rendering;

/// <summary>
/// Caméra 2D pour naviguer dans le monde.
/// </summary>
public class Camera2D
{
    private Vector2 _position;
    private float _zoom;
    private float _rotation;
    private readonly Viewport _viewport;

    /// <summary>
    /// Position de la caméra dans le monde (en pixels).
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    /// Niveau de zoom (1.0 = normal, 2.0 = zoom x2, 0.5 = dézoom x2).
    /// </summary>
    public float Zoom
    {
        get => _zoom;
        set => _zoom = MathHelper.Clamp(value, 0.1f, 5.0f);
    }

    /// <summary>
    /// Rotation de la caméra en radians.
    /// </summary>
    public float Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    /// <summary>
    /// Origine de la caméra (centre de l'écran).
    /// </summary>
    public Vector2 Origin => new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);

    /// <summary>
    /// Matrice de transformation pour le rendu.
    /// </summary>
    public Matrix TransformMatrix
    {
        get
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));
        }
    }

    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        _position = Vector2.Zero;
        _zoom = 1.0f;
        _rotation = 0f;
    }

    /// <summary>
    /// Déplace la caméra d'un certain offset.
    /// </summary>
    public void Move(Vector2 offset)
    {
        _position += offset;
    }

    /// <summary>
    /// Centre la caméra sur une position.
    /// </summary>
    public void CenterOn(Vector2 worldPosition)
    {
        _position = worldPosition;
    }

    /// <summary>
    /// Convertit une position d'écran en position monde.
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        return Vector2.Transform(screenPosition, Matrix.Invert(TransformMatrix));
    }

    /// <summary>
    /// Convertit une position monde en position écran.
    /// </summary>
    public Vector2 WorldToScreen(Vector2 worldPosition)
    {
        return Vector2.Transform(worldPosition, TransformMatrix);
    }

    /// <summary>
    /// Retourne les limites visibles de la caméra dans le monde (en pixels).
    /// </summary>
    public Rectangle GetVisibleArea()
    {
        var inverseMatrix = Matrix.Invert(TransformMatrix);

        var topLeft = Vector2.Transform(Vector2.Zero, inverseMatrix);
        var topRight = Vector2.Transform(new Vector2(_viewport.Width, 0), inverseMatrix);
        var bottomLeft = Vector2.Transform(new Vector2(0, _viewport.Height), inverseMatrix);
        var bottomRight = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseMatrix);

        var min = new Vector2(
            MathHelper.Min(topLeft.X, MathHelper.Min(topRight.X, MathHelper.Min(bottomLeft.X, bottomRight.X))),
            MathHelper.Min(topLeft.Y, MathHelper.Min(topRight.Y, MathHelper.Min(bottomLeft.Y, bottomRight.Y)))
        );

        var max = new Vector2(
            MathHelper.Max(topLeft.X, MathHelper.Max(topRight.X, MathHelper.Max(bottomLeft.X, bottomRight.X))),
            MathHelper.Max(topLeft.Y, MathHelper.Max(topRight.Y, MathHelper.Max(bottomLeft.Y, bottomRight.Y)))
        );

        return new Rectangle(
            (int)min.X,
            (int)min.Y,
            (int)(max.X - min.X),
            (int)(max.Y - min.Y)
        );
    }
}
