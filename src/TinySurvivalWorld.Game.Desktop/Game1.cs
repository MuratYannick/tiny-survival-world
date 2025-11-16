using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TinySurvivalWorld.Core.World;
using TinySurvivalWorld.Game.Desktop.Entities;
using TinySurvivalWorld.Game.Desktop.Rendering;
using XnaGame = Microsoft.Xna.Framework.Game;

namespace TinySurvivalWorld.Game.Desktop;

public class Game1 : XnaGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;

    // Système monde
    private ChunkManager? _chunkManager;
    private TileRenderer? _tileRenderer;

    // Personnage joueur
    private PlayerCharacter? _player;
    private PlayerRenderer? _playerRenderer;

    // Caméra
    private Camera2D? _camera;
    private bool _freeCameraMode = false; // false = suit le joueur, true = caméra libre

    // Légende
    private LegendRenderer? _legendRenderer;
    private bool _showLegend = false;

    // Input
    private KeyboardState _previousKeyboardState;
    private const float CameraSpeed = 300f; // Pixels par seconde
    private const float ZoomSpeed = 0.5f;

    // Debug
#pragma warning disable CS0649 // Field is never assigned (debug font is optional)
    private SpriteFont? _debugFont;
#pragma warning restore CS0649
    private bool _showDebugInfo = true;
    private bool _showChunkGrid = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Configurer la résolution
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // Créer la caméra
        _camera = new Camera2D(GraphicsDevice.Viewport);

        // Créer le monde avec une seed aléatoire (ou fixe pour tests)
        long worldSeed = 12345; // Seed fixe pour le développement
        _chunkManager = new ChunkManager(worldSeed);

        // Trouver un point de spawn valide
        var spawnPoint = _chunkManager.FindSpawnPoint();
        var spawnPosition = new Vector2(
            spawnPoint.x * WorldConstants.TileSize + WorldConstants.TileSize / 2,
            spawnPoint.y * WorldConstants.TileSize + WorldConstants.TileSize / 2
        );

        // Créer le personnage joueur au spawn point
        _player = new PlayerCharacter(_chunkManager, spawnPosition);

        // Centrer la caméra sur le joueur
        _camera.CenterOn(_player.Position);

        // Précharger les chunks autour du spawn
        _chunkManager.LoadChunksAroundPosition(spawnPoint.x, spawnPoint.y);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Créer les renderers
        if (_chunkManager != null)
        {
            _tileRenderer = new TileRenderer(GraphicsDevice, _chunkManager);
        }

        _playerRenderer = new PlayerRenderer(GraphicsDevice);
        _legendRenderer = new LegendRenderer(GraphicsDevice);

        // Charger la font pour le debug et la légende
        try
        {
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _legendRenderer.SetFont(_debugFont);
        }
        catch
        {
            // La font n'est pas obligatoire, on continue sans
        }
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // Quitter
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        // Toggle debug info
        if (keyboardState.IsKeyDown(Keys.F1) && !_previousKeyboardState.IsKeyDown(Keys.F1))
            _showDebugInfo = !_showDebugInfo;

        // Toggle chunk grid
        if (keyboardState.IsKeyDown(Keys.F2) && !_previousKeyboardState.IsKeyDown(Keys.F2))
            _showChunkGrid = !_showChunkGrid;

        // Toggle free camera mode
        if (keyboardState.IsKeyDown(Keys.F3) && !_previousKeyboardState.IsKeyDown(Keys.F3))
            _freeCameraMode = !_freeCameraMode;

        // Toggle legend
        if (keyboardState.IsKeyDown(Keys.L) && !_previousKeyboardState.IsKeyDown(Keys.L))
            _showLegend = !_showLegend;

        // Gestion de la caméra
        if (_camera != null)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_freeCameraMode)
            {
                // Mode caméra libre (flèches directionnelles uniquement)
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y += 1;
                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                    _camera.Move(movement * CameraSpeed * deltaTime / _camera.Zoom);
                }
            }
            else
            {
                // Mode suivi du joueur - mettre à jour le personnage
                if (_player != null)
                {
                    _player.Update(gameTime, keyboardState);
                    _camera.CenterOn(_player.Position);
                }
            }

            // Zoom (+/-) - fonctionne dans les deux modes
            if (keyboardState.IsKeyDown(Keys.OemMinus) || keyboardState.IsKeyDown(Keys.Subtract))
                _camera.Zoom -= ZoomSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.OemPlus) || keyboardState.IsKeyDown(Keys.Add))
                _camera.Zoom += ZoomSpeed * deltaTime;

            // Charger les chunks autour de la position de référence (joueur ou caméra)
            if (_chunkManager != null)
            {
                Vector2 referencePosition = _freeCameraMode ? _camera.Position : (_player?.Position ?? _camera.Position);
                int referenceTileX = (int)(referencePosition.X / WorldConstants.TileSize);
                int referenceTileY = (int)(referencePosition.Y / WorldConstants.TileSize);
                _chunkManager.LoadChunksAroundPosition(referenceTileX, referenceTileY);
            }
        }

        _previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 20, 30)); // Fond sombre

        if (_spriteBatch == null || _camera == null || _tileRenderer == null)
            return;

        // Dessiner le monde avec la transformation de la caméra
        _spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp, // Pixel art
            transformMatrix: _camera.TransformMatrix
        );

        _tileRenderer.Draw(_spriteBatch, _camera);

        if (_showChunkGrid)
        {
            _tileRenderer.DrawChunkGrid(_spriteBatch, _camera);
        }

        // Dessiner le personnage joueur
        if (_player != null && _playerRenderer != null)
        {
            _playerRenderer.Draw(_spriteBatch, _player);
        }

        _spriteBatch.End();

        // Debug info (sans transformation de caméra)
        if (_showDebugInfo)
        {
            DrawDebugInfo(gameTime);
        }

        // Légende des terrains (sans transformation de caméra)
        if (_showLegend && _legendRenderer != null)
        {
            try
            {
                _spriteBatch.Begin();
                _legendRenderer.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                _spriteBatch.End();
            }
            catch (Exception ex)
            {
                // Si erreur lors de l'affichage de la légende, désactiver et continuer
                _showLegend = false;
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'affichage de la légende: {ex.Message}");
            }
        }

        base.Draw(gameTime);
    }

    private void DrawDebugInfo(GameTime gameTime)
    {
        if (_spriteBatch == null || _camera == null || _tileRenderer == null || _chunkManager == null)
            return;

        _spriteBatch.Begin();

        int y = 10;
        int lineHeight = 20;

        // Si pas de font, dessiner un rectangle de fond pour indiquer la zone de debug
        var debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        debugTexture.SetData(new[] { new Color(0, 0, 0, 150) });

        _spriteBatch.Draw(debugTexture, new Rectangle(5, 5, 400, 240), Color.White);

        // Afficher les infos textuelles (si on a une font)
        if (_debugFont != null)
        {
            DrawDebugText($"FPS: {1.0f / gameTime.ElapsedGameTime.TotalSeconds:F0}", 10, y);
            y += lineHeight;

            if (_player != null)
            {
                DrawDebugText($"Player: {_player.Position.X:F0}, {_player.Position.Y:F0}", 10, y);
                y += lineHeight;
            }

            DrawDebugText($"Camera: {_camera.Position.X:F0}, {_camera.Position.Y:F0}", 10, y);
            y += lineHeight;
            DrawDebugText($"Zoom: {_camera.Zoom:F2}x", 10, y);
            y += lineHeight;
            DrawDebugText($"Camera Mode: {(_freeCameraMode ? "FREE" : "FOLLOW PLAYER")}", 10, y);
            y += lineHeight;
            DrawDebugText($"Chunks loaded: {_chunkManager.LoadedChunkCount}", 10, y);
            y += lineHeight;
            DrawDebugText($"Chunks rendered: {_tileRenderer.ChunksRenderedLastFrame}", 10, y);
            y += lineHeight;
            DrawDebugText($"Tiles rendered: {_tileRenderer.TilesRenderedLastFrame}", 10, y);
            y += lineHeight;

            if (_freeCameraMode)
            {
                DrawDebugText("Controls: Arrows=Move Cam, +/-=Zoom", 10, y);
            }
            else
            {
                DrawDebugText("Controls: ZQSD/Arrows=Move, +/-=Zoom", 10, y);
            }
            y += lineHeight;
            DrawDebugText("F1=Debug, F2=Grid, F3=Free Cam, L=Legend", 10, y);
            y += lineHeight;
            DrawDebugText("ESC=Quit", 10, y);
        }

        _spriteBatch.End();
        debugTexture.Dispose();
    }

    private void DrawDebugText(string text, int x, int y)
    {
        if (_debugFont != null && _spriteBatch != null)
        {
            _spriteBatch.DrawString(_debugFont, text, new Vector2(x, y), Color.White);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _tileRenderer?.Dispose();
            _playerRenderer?.Dispose();
            _legendRenderer?.Dispose();
        }

        base.Dispose(disposing);
    }
}
