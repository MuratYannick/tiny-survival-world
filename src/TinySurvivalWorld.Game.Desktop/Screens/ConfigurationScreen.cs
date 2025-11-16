using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TinySurvivalWorld.Core.World;
using TinySurvivalWorld.Game.Desktop.Rendering;
using TinySurvivalWorld.Game.Desktop.Utilities;

namespace TinySurvivalWorld.Game.Desktop.Screens;

/// <summary>
/// Ecran de configuration pour ajuster les parametres de generation du monde (DevMode uniquement).
/// </summary>
public class ConfigurationScreen
{
    private readonly GraphicsDevice _graphicsDevice;
    private SpriteFont? _font;
    private Texture2D? _pixelTexture;

    // Configuration
    private WorldGenerationConfig _config;
    private string _seedInput = "";
    private long _currentSeed;

    // UI State
    private int _selectedParam = 0;
    private const int TotalParams = 17; // 15 params + seed input + bouton valider

    // Preview
    private ChunkManager? _previewChunkManager;
    private float _previewZoom = 1.0f;
    private const float MinZoom = 0.5f;
    private const float MaxZoom = 4.0f;
    private const float ZoomSpeed = 0.1f;

    // Parametres ajustables
    private readonly ConfigParam[] _params;

    private KeyboardState _previousKeyboardState;

    // Input throttling for value adjustment
    private double _lastAdjustmentTime = 0;
    private const double AdjustmentCooldown = 0.15; // 150ms entre chaque ajustement

    public ConfigurationScreen(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _config = WorldGenerationConfig.Default.Clone();

        // Generer un seed aleatoire initial (fixe pour cette session de configuration)
        _currentSeed = DateTime.UtcNow.Ticks;

        // Definition des parametres ajustables
        _params = new[]
        {
            new ConfigParam("Elevation Octaves", () => _config.ElevationOctaves, v => _config.ElevationOctaves = v, 1, 8, 1),
            new ConfigParam("Elevation Persistence", () => _config.ElevationPersistence, v => _config.ElevationPersistence = v, 0.1f, 1.0f, 0.05f),
            new ConfigParam("Elevation Lacunarity", () => _config.ElevationLacunarity, v => _config.ElevationLacunarity = v, 1.0f, 4.0f, 0.1f),
            new ConfigParam("Elevation Scale", () => _config.ElevationScale, v => _config.ElevationScale = v, 0.1f, 2.0f, 0.05f),
            new ConfigParam("Elevation Offset", () => _config.ElevationOffset, v => _config.ElevationOffset = v, 0.0f, 1.0f, 0.05f),

            new ConfigParam("Moisture Octaves", () => _config.MoistureOctaves, v => _config.MoistureOctaves = v, 1, 8, 1),
            new ConfigParam("Moisture Persistence", () => _config.MoisturePersistence, v => _config.MoisturePersistence = v, 0.1f, 1.0f, 0.05f),
            new ConfigParam("Moisture Lacunarity", () => _config.MoistureLacunarity, v => _config.MoistureLacunarity = v, 1.0f, 4.0f, 0.1f),
            new ConfigParam("Moisture Scale", () => _config.MoistureScale, v => _config.MoistureScale = v, 0.1f, 2.0f, 0.05f),
            new ConfigParam("Moisture Offset", () => _config.MoistureOffset, v => _config.MoistureOffset = v, 0.0f, 1.0f, 0.05f),

            new ConfigParam("Temperature Octaves", () => _config.TemperatureOctaves, v => _config.TemperatureOctaves = v, 1, 8, 1),
            new ConfigParam("Temperature Persistence", () => _config.TemperaturePersistence, v => _config.TemperaturePersistence = v, 0.1f, 1.0f, 0.05f),
            new ConfigParam("Temperature Lacunarity", () => _config.TemperatureLacunarity, v => _config.TemperatureLacunarity = v, 1.0f, 4.0f, 0.1f),
            new ConfigParam("Temperature Scale", () => _config.TemperatureScale, v => _config.TemperatureScale = v, 0.1f, 2.0f, 0.05f),
            new ConfigParam("Temperature Offset", () => _config.TemperatureOffset, v => _config.TemperatureOffset = v, 0.0f, 1.0f, 0.05f),
        };

        CreatePixelTexture();
        RegeneratePreview();
    }

    public void SetFont(SpriteFont? font)
    {
        _font = font;
    }

    private void CreatePixelTexture()
    {
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        double currentTime = gameTime.TotalGameTime.TotalSeconds;

        // Navigation verticale
        if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
        {
            _selectedParam = (_selectedParam - 1 + TotalParams) % TotalParams;
        }
        else if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
        {
            _selectedParam = (_selectedParam + 1) % TotalParams;
        }

        // Ajustement des valeurs ou saisie seed
        if (_selectedParam < _params.Length)
        {
            // Parametres de generation - avec throttling pour eviter ajustement trop rapide
            var param = _params[_selectedParam];
            bool changed = false;

            if (currentTime - _lastAdjustmentTime >= AdjustmentCooldown)
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    param.Decrease();
                    changed = true;
                    _lastAdjustmentTime = currentTime;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    param.Increase();
                    changed = true;
                    _lastAdjustmentTime = currentTime;
                }
            }

            if (changed)
            {
                RegeneratePreview();
            }
        }
        else if (_selectedParam == _params.Length)
        {
            // Saisie du seed
            HandleSeedInput(keyboardState);
        }

        // Zoom de la preview
        if (keyboardState.IsKeyDown(Keys.OemPlus) || keyboardState.IsKeyDown(Keys.Add))
        {
            _previewZoom = Math.Clamp(_previewZoom + ZoomSpeed, MinZoom, MaxZoom);
        }
        else if (keyboardState.IsKeyDown(Keys.OemMinus) || keyboardState.IsKeyDown(Keys.Subtract))
        {
            _previewZoom = Math.Clamp(_previewZoom - ZoomSpeed, MinZoom, MaxZoom);
        }

        // Reset config
        if (keyboardState.IsKeyDown(Keys.R) && !_previousKeyboardState.IsKeyDown(Keys.R))
        {
            _config = WorldGenerationConfig.Default.Clone();
            RegeneratePreview();
        }

        // Touche S pour generer un nouveau seed aleatoire
        if (keyboardState.IsKeyDown(Keys.S) && !_previousKeyboardState.IsKeyDown(Keys.S))
        {
            _currentSeed = DateTime.UtcNow.Ticks;
            RegeneratePreview();
        }

        _previousKeyboardState = keyboardState;
    }

    private void HandleSeedInput(KeyboardState keyboardState)
    {
        // Gestion simplifiee de la saisie du seed (chiffres uniquement)
        var pressedKeys = keyboardState.GetPressedKeys();

        foreach (var key in pressedKeys)
        {
            if (_previousKeyboardState.IsKeyUp(key))
            {
                // Backspace
                if (key == Keys.Back && _seedInput.Length > 0)
                {
                    _seedInput = _seedInput.Substring(0, _seedInput.Length - 1);
                }
                // Chiffres
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    int digit = key - Keys.D0;
                    if (_seedInput.Length < 18) // Limite pour long
                    {
                        _seedInput += digit.ToString();
                    }
                }
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                {
                    int digit = key - Keys.NumPad0;
                    if (_seedInput.Length < 18)
                    {
                        _seedInput += digit.ToString();
                    }
                }
            }
        }
    }

    private void RegeneratePreview()
    {
        // Utiliser le seed actuel (fixe sauf si regenere par touche S)
        _previewChunkManager = new ChunkManager(_currentSeed, _config.Clone());

        // Precharger quelques chunks pour la preview
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                _previewChunkManager.GetOrCreateChunk(x, y);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
        if (_pixelTexture == null)
            return;

        // Fond noir semi-transparent
        spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, screenWidth, screenHeight), new Color(0, 0, 0, 230));

        // Split screen: gauche = preview, droite = controls
        int splitX = screenWidth / 2;

        // === PREVIEW (gauche) ===
        DrawPreview(spriteBatch, 0, 0, splitX, screenHeight);

        // === CONTROLS (droite) ===
        DrawControls(spriteBatch, splitX, 0, splitX, screenHeight);
    }

    private void DrawPreview(SpriteBatch spriteBatch, int x, int y, int width, int height)
    {
        if (_previewChunkManager == null || _pixelTexture == null)
            return;

        // Dessiner une zone de preview centree - tiles 2x plus petites pour voir plus de terrain
        int tileSize = (int)(WorldConstants.TileSize * _previewZoom / 2f);
        int centerX = x + width / 2;
        int centerY = y + height / 2;

        int tilesX = width / tileSize + 2;
        int tilesY = height / tileSize + 2;

        for (int tx = -tilesX / 2; tx < tilesX / 2; tx++)
        {
            for (int ty = -tilesY / 2; ty < tilesY / 2; ty++)
            {
                var tile = _previewChunkManager.GetTile(tx, ty);
                if (tile != null)
                {
                    var tileColor = TileColors.GetColor(tile.Type);
                    var destRect = new Rectangle(
                        centerX + tx * tileSize,
                        centerY + ty * tileSize,
                        tileSize,
                        tileSize
                    );
                    spriteBatch.Draw(_pixelTexture, destRect, tileColor);
                }
            }
        }

        // Titre
        if (_font != null)
        {
            spriteBatch.DrawString(_font, "Preview (Zoom: +/-)",
                new Vector2(x + 10, y + 10), Color.White);
        }
    }

    private void DrawControls(SpriteBatch spriteBatch, int x, int y, int width, int height)
    {
        if (_font == null || _pixelTexture == null)
            return;

        // Decalage vers la droite pour eviter chevaucher la preview
        int offsetX = 30;
        int currentY = y + 20;
        int lineHeight = 30;

        // Titre
        spriteBatch.DrawString(_font, "=== WORLD GENERATION CONFIG ===",
            new Vector2(x + offsetX, currentY), Color.Yellow);
        currentY += lineHeight + 10;

        // Parametres
        for (int i = 0; i < _params.Length; i++)
        {
            var param = _params[i];
            bool isSelected = (_selectedParam == i);
            var textColor = isSelected ? Color.Cyan : Color.White;

            string valueText = param.GetFormattedValue();
            string line = $"{(isSelected ? ">" : " ")} {param.Name}: {valueText}";

            spriteBatch.DrawString(_font, line, new Vector2(x + offsetX, currentY), textColor);
            currentY += lineHeight;
        }

        currentY += 10;

        // Seed input
        bool seedSelected = (_selectedParam == _params.Length);
        var seedColor = seedSelected ? Color.Cyan : Color.White;
        string seedText = string.IsNullOrEmpty(_seedInput) ? "(random)" : _seedInput;
        spriteBatch.DrawString(_font, $"{(seedSelected ? ">" : " ")} Seed: {seedText}",
            new Vector2(x + offsetX, currentY), seedColor);
        currentY += lineHeight;

        // Bouton valider
        bool startSelected = (_selectedParam == _params.Length + 1);
        var startColor = startSelected ? Color.Green : Color.White;
        spriteBatch.DrawString(_font, $"{(startSelected ? ">" : " ")} [ START GAME ]",
            new Vector2(x + offsetX, currentY), startColor);
        currentY += lineHeight + 20;

        // Instructions
        spriteBatch.DrawString(_font, "Controls:",
            new Vector2(x + offsetX, currentY), Color.Gray);
        currentY += lineHeight;

        spriteBatch.DrawString(_font, "  Up/Down: Navigate",
            new Vector2(x + offsetX, currentY), Color.Gray);
        currentY += lineHeight - 5;

        spriteBatch.DrawString(_font, "  Left/Right: Adjust value",
            new Vector2(x + offsetX, currentY), Color.Gray);
        currentY += lineHeight - 5;

        spriteBatch.DrawString(_font, "  R: Reset to default",
            new Vector2(x + offsetX, currentY), Color.Gray);
        currentY += lineHeight - 5;

        spriteBatch.DrawString(_font, "  S: New random seed",
            new Vector2(x + offsetX, currentY), Color.Gray);
        currentY += lineHeight - 5;

        spriteBatch.DrawString(_font, "  Enter: Start game",
            new Vector2(x + offsetX, currentY), Color.Gray);
    }

    public bool ShouldStartGame(KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        // Start si Enter sur le bouton START ou sur n'importe quel parametre
        return keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter);
    }

    public (long seed, WorldGenerationConfig config) GetGameSettings()
    {
        long seed;
        if (!string.IsNullOrEmpty(_seedInput) && long.TryParse(_seedInput, out long parsedSeed))
        {
            seed = parsedSeed;
        }
        else
        {
            seed = DateTime.UtcNow.Ticks;
        }

        return (seed, _config.Clone());
    }

    public void Dispose()
    {
        _pixelTexture?.Dispose();
    }

    /// <summary>
    /// Classe helper pour gerer un parametre de configuration.
    /// </summary>
    private class ConfigParam
    {
        public string Name { get; }
        private readonly Func<float> _getter;
        private readonly Action<int> _setterInt;
        private readonly Action<float> _setterFloat;
        private readonly float _min;
        private readonly float _max;
        private readonly float _step;
        private readonly bool _isInt;

        public ConfigParam(string name, Func<int> getter, Action<int> setter, int min, int max, int step)
        {
            Name = name;
            _getter = () => getter();
            _setterInt = setter;
            _setterFloat = null!;
            _min = min;
            _max = max;
            _step = step;
            _isInt = true;
        }

        public ConfigParam(string name, Func<float> getter, Action<float> setter, float min, float max, float step)
        {
            Name = name;
            _getter = getter;
            _setterInt = null!;
            _setterFloat = setter;
            _min = min;
            _max = max;
            _step = step;
            _isInt = false;
        }

        public void Increase()
        {
            float current = _getter();
            float newValue = Math.Clamp(current + _step, _min, _max);

            if (_isInt)
                _setterInt((int)newValue);
            else
                _setterFloat(newValue);
        }

        public void Decrease()
        {
            float current = _getter();
            float newValue = Math.Clamp(current - _step, _min, _max);

            if (_isInt)
                _setterInt((int)newValue);
            else
                _setterFloat(newValue);
        }

        public string GetFormattedValue()
        {
            float value = _getter();
            return _isInt ? $"{(int)value}" : $"{value:F2}";
        }
    }
}
