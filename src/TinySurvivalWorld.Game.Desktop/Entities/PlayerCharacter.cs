using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TinySurvivalWorld.Core.World;

namespace TinySurvivalWorld.Game.Desktop.Entities;

/// <summary>
/// Représente le personnage joueur dans le monde de jeu.
/// </summary>
public class PlayerCharacter
{
    private readonly ChunkManager _chunkManager;

    /// <summary>
    /// Position du personnage dans le monde (en pixels).
    /// </summary>
    public Vector2 Position { get; private set; }

    /// <summary>
    /// Vitesse de déplacement en pixels par seconde.
    /// </summary>
    public float MoveSpeed { get; set; } = 150f;

    /// <summary>
    /// Taille du personnage (largeur et hauteur en pixels).
    /// </summary>
    public int Size { get; set; } = 24;

    /// <summary>
    /// Rectangle de collision du personnage.
    /// </summary>
    public Rectangle CollisionBox
    {
        get
        {
            return new Rectangle(
                (int)(Position.X - Size / 2),
                (int)(Position.Y - Size / 2),
                Size,
                Size
            );
        }
    }

    public PlayerCharacter(ChunkManager chunkManager, Vector2 initialPosition)
    {
        _chunkManager = chunkManager;
        Position = initialPosition;
    }

    /// <summary>
    /// Met à jour le personnage (gestion des inputs et mouvement).
    /// </summary>
    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 movement = Vector2.Zero;

        // Inputs de mouvement (ZQSD ou flèches)
        if (keyboardState.IsKeyDown(Keys.Z) || keyboardState.IsKeyDown(Keys.Up))
            movement.Y -= 1;
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            movement.Y += 1;
        if (keyboardState.IsKeyDown(Keys.Q) || keyboardState.IsKeyDown(Keys.Left))
            movement.X -= 1;
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            movement.X += 1;

        // Normaliser le mouvement diagonal
        if (movement != Vector2.Zero)
        {
            movement.Normalize();
            TryMove(movement * MoveSpeed * deltaTime);
        }
    }

    /// <summary>
    /// Tente de déplacer le personnage, en vérifiant les collisions.
    /// </summary>
    private void TryMove(Vector2 movement)
    {
        // Essayer le mouvement sur l'axe X
        Vector2 newPositionX = Position + new Vector2(movement.X, 0);
        if (CanMoveTo(newPositionX))
        {
            Position = newPositionX;
        }

        // Essayer le mouvement sur l'axe Y
        Vector2 newPositionY = Position + new Vector2(0, movement.Y);
        if (CanMoveTo(newPositionY))
        {
            Position = newPositionY;
        }
    }

    /// <summary>
    /// Vérifie si le personnage peut se déplacer vers une position donnée.
    /// </summary>
    private bool CanMoveTo(Vector2 position)
    {
        // Calculer les coins de la collision box à la nouvelle position
        int halfSize = Size / 2;
        int left = (int)(position.X - halfSize);
        int right = (int)(position.X + halfSize);
        int top = (int)(position.Y - halfSize);
        int bottom = (int)(position.Y + halfSize);

        // Vérifier les 4 coins du personnage
        return IsTileWalkable(left, top) &&
               IsTileWalkable(right, top) &&
               IsTileWalkable(left, bottom) &&
               IsTileWalkable(right, bottom);
    }

    /// <summary>
    /// Vérifie si une tile aux coordonnées pixel données est marchable.
    /// </summary>
    private bool IsTileWalkable(int pixelX, int pixelY)
    {
        int tileX = pixelX / WorldConstants.TileSize;
        int tileY = pixelY / WorldConstants.TileSize;

        var (chunkX, chunkY) = Chunk.WorldToChunkCoords(tileX, tileY);
        var chunk = _chunkManager.GetOrCreateChunk(chunkX, chunkY);

        if (!chunk.IsGenerated)
            return false;

        var (localX, localY) = Chunk.WorldToLocalCoords(tileX, tileY);
        var tile = chunk.GetTile(localX, localY);

        return tile?.IsWalkable ?? false;
    }
}
