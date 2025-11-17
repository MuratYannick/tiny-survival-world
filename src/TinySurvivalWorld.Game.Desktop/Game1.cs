using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Extensions.Configuration;
using TinySurvivalWorld.Core.World;
using TinySurvivalWorld.Core.Time;
using TinySurvivalWorld.Game.Desktop.Entities;
using TinySurvivalWorld.Game.Desktop.Rendering;
using TinySurvivalWorld.Game.Desktop.Screens;
using TinySurvivalWorld.Game.Desktop.Utilities;
using XnaGame = Microsoft.Xna.Framework.Game;

namespace TinySurvivalWorld.Game.Desktop;

public class Game1 : XnaGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;

    // Configuration
    private bool _devMode = false;
    private bool _inConfigScreen = false;
    private ConfigurationScreen? _configScreen;

    // Système monde
    private ChunkManager? _chunkManager;
    private TileRenderer? _tileRenderer;
    private WorldGenerationConfig? _worldConfig;
    private long _worldSeed;

    // Personnage joueur
    private PlayerCharacter? _player;
    private PlayerRenderer? _playerRenderer;

    // Caméra
    private Camera2D? _camera;
    private bool _freeCameraMode = false; // false = suit le joueur, true = caméra libre

    // Légende
    private LegendRenderer? _legendRenderer;
    private bool _showLegend = false;

    // Gestion du temps
    private TimeManager? _timeManager;
    private DayNightCycleRenderer? _dayNightRenderer;

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
        GameLogger.Info("=== TINY SURVIVAL WORLD - DÉMARRAGE ===");
        GameLogger.Info($"Fichier de log: {GameLogger.GetLogFilePath()}");

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Configurer la résolution
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        // Charger la configuration
        LoadConfiguration();

        GameLogger.Info($"Game1 constructor - Terminé (DevMode={_devMode})");
    }

    private void LoadConfiguration()
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            _devMode = config.GetValue<bool>("GameSettings:DevMode", false);
            GameLogger.Info($"Configuration chargee - DevMode={_devMode}");
        }
        catch (Exception ex)
        {
            GameLogger.Error("Erreur lors du chargement de la configuration", ex);
            _devMode = false;
        }
    }

    protected override void Initialize()
    {
        if (_devMode)
        {
            // Mode développement : afficher l'ecran de configuration
            _inConfigScreen = true;
            _configScreen = new ConfigurationScreen(GraphicsDevice);
            GameLogger.Info("Initialize - Mode configuration (DevMode)");
        }
        else
        {
            // Mode normal : démarrer le jeu directement avec config par défaut
            StartGame(DateTime.UtcNow.Ticks, WorldGenerationConfig.Default);
            GameLogger.Info("Initialize - Mode jeu direct");
        }

        base.Initialize();
    }

    private void StartGame(long seed, WorldGenerationConfig config)
    {
        GameLogger.Info($"StartGame - Seed={seed}");

        _worldSeed = seed;
        _worldConfig = config;
        _inConfigScreen = false;

        // Créer la caméra
        _camera = new Camera2D(GraphicsDevice.Viewport);

        // Créer le monde avec la config
        _chunkManager = new ChunkManager(_worldSeed, _worldConfig);

        // Trouver un point de spawn valide
        var spawnPoint = _chunkManager.FindSpawnPoint();
        var spawnPosition = new Vector2(
            spawnPoint.x * WorldConstants.TileSize + WorldConstants.TileSize / 2,
            spawnPoint.y * WorldConstants.TileSize + WorldConstants.TileSize / 2
        );

        // Créer le personnage joueur au spawn point
        _player = new PlayerCharacter(_chunkManager, spawnPosition);

        // Initialiser le système de temps
        _timeManager = new TimeManager();
        GameLogger.Info($"TimeManager initialisé - Heure actuelle IG: {_timeManager.GetFormattedDateTime()}");
        GameLogger.Info($"Jour {_timeManager.CurrentDay} - {_timeManager.GetTimeOfDayName()}");

        // Initialiser le renderer de cycle jour/nuit
        _dayNightRenderer = new DayNightCycleRenderer(GraphicsDevice, _timeManager);
        GameLogger.Info("DayNightCycleRenderer initialisé");

        // Centrer la caméra sur le joueur
        _camera.CenterOn(_player.Position);

        // Précharger les chunks autour du spawn
        _chunkManager.LoadChunksAroundPosition((int)spawnPoint.x, (int)spawnPoint.y);

        GameLogger.Info($"StartGame - Spawn point: ({spawnPoint.x}, {spawnPoint.y})");
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Charger la font pour le debug, la légende et la config
        try
        {
            _debugFont = Content.Load<SpriteFont>("DebugFont");
        }
        catch
        {
            // La font n'est pas obligatoire, on continue sans
        }

        // Créer les renderers
        if (_chunkManager != null)
        {
            _tileRenderer = new TileRenderer(GraphicsDevice, _chunkManager);
        }

        _playerRenderer = new PlayerRenderer(GraphicsDevice);
        _legendRenderer = new LegendRenderer(GraphicsDevice);
        _legendRenderer.SetFont(_debugFont);

        // Passer la font au ConfigurationScreen si en mode config
        if (_configScreen != null)
        {
            _configScreen.SetFont(_debugFont);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // Quitter
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (_inConfigScreen && _configScreen != null)
        {
            // Mode configuration
            _configScreen.Update(gameTime, keyboardState);

            // Check si on doit démarrer le jeu
            if (_configScreen.ShouldStartGame(keyboardState, _previousKeyboardState))
            {
                var (seed, config) = _configScreen.GetGameSettings();
                GameLogger.Info($"Configuration terminee - Demarrage du jeu avec seed={seed}");
                StartGame(seed, config);

                // Recreer les renderers pour le monde
                if (_chunkManager != null)
                {
                    _tileRenderer = new TileRenderer(GraphicsDevice, _chunkManager);
                }
            }
        }
        else
        {
            // Mode jeu normal
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
            {
                _showLegend = !_showLegend;
                GameLogger.Info($"Toggle legend - Nouvelle valeur: {_showLegend}");
            }

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

            // Mettre à jour le système de temps
            if (_timeManager != null)
            {
                _timeManager.Update();
            }
            }
        } // Fin du else (mode jeu normal)

        _previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 20, 30)); // Fond sombre

        if (_spriteBatch == null)
            return;

        if (_inConfigScreen && _configScreen != null)
        {
            // Mode configuration - afficher l'ecran de config
            _spriteBatch.Begin();
            _configScreen.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _spriteBatch.End();
        }
        else if (_camera != null && _tileRenderer != null)
        {
            // Mode jeu normal - afficher le monde
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

            // Overlay de cycle jour/nuit (sans transformation de caméra)
            if (_dayNightRenderer != null)
            {
                _spriteBatch.Begin();
                _dayNightRenderer.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                _spriteBatch.End();
            }

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
                    GameLogger.Info("Game1.Draw() - Début affichage légende");
                    _spriteBatch.Begin();
                    GameLogger.Info("Game1.Draw() - SpriteBatch.Begin() appelé");

                    _legendRenderer.Draw(_spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    GameLogger.Info("Game1.Draw() - LegendRenderer.Draw() terminé");

                    _spriteBatch.End();
                    GameLogger.Info("Game1.Draw() - SpriteBatch.End() appelé");
                }
                catch (Exception ex)
                {
                    // Si erreur lors de l'affichage de la légende, désactiver et continuer
                    GameLogger.Error("Game1.Draw() - ERREUR lors de l'affichage de la légende", ex);
                    _showLegend = false;
                }
            }
        } // Fin du else if (mode jeu normal)

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

        _spriteBatch.Draw(debugTexture, new Rectangle(5, 5, 450, 300), Color.White);

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

            // Informations de temps
            if (_timeManager != null)
            {
                y += lineHeight / 2; // Petit espace
                DrawDebugText($"Time: {_timeManager.GetFormattedTime()}", 10, y);
                y += lineHeight;
                DrawDebugText($"Period: {_timeManager.GetTimeOfDayName()}", 10, y);
                y += lineHeight;
            }

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
            y += lineHeight;
            y += lineHeight;
            DrawDebugText($"Log: {GameLogger.GetLogFilePath()}", 10, y);
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
            _configScreen?.Dispose();
            _dayNightRenderer?.Dispose();
        }

        base.Dispose(disposing);
    }
}
