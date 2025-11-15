using TinySurvivalWorld.Core.Enums;

namespace TinySurvivalWorld.Core.Models;

/// <summary>
/// Représente un monde/sauvegarde de jeu.
/// Chaque monde est une instance unique avec sa propre génération procédurale.
/// </summary>
public class World
{
    /// <summary>
    /// Identifiant unique du monde.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nom du monde (unique).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Seed utilisée pour la génération procédurale du monde.
    /// Permet de recréer le même monde avec la même seed.
    /// </summary>
    public long Seed { get; set; }

    /// <summary>
    /// Temps de jeu écoulé en millisecondes depuis la création du monde.
    /// </summary>
    public long GameTime { get; set; } = 0;

    /// <summary>
    /// Niveau de difficulté du monde.
    /// </summary>
    public Difficulty Difficulty { get; set; } = Difficulty.Normal;

    /// <summary>
    /// Indique si le monde est en mode hardcore (permadeath).
    /// </summary>
    public bool IsHardcore { get; set; } = false;

    /// <summary>
    /// Taille du monde en chunks (largeur).
    /// 0 = infini (génération à la demande).
    /// </summary>
    public int WorldSizeX { get; set; } = 0;

    /// <summary>
    /// Taille du monde en chunks (hauteur).
    /// 0 = infini (génération à la demande).
    /// </summary>
    public int WorldSizeY { get; set; } = 0;

    /// <summary>
    /// Position de spawn X par défaut pour les nouveaux joueurs.
    /// </summary>
    public float SpawnPointX { get; set; } = 0f;

    /// <summary>
    /// Position de spawn Y par défaut pour les nouveaux joueurs.
    /// </summary>
    public float SpawnPointY { get; set; } = 0f;

    // Métadonnées

    /// <summary>
    /// Date de création du monde (réelle).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Dernière date d'accès au monde.
    /// </summary>
    public DateTime LastPlayed { get; set; }

    /// <summary>
    /// Version du jeu utilisée pour créer ce monde.
    /// </summary>
    public string GameVersion { get; set; } = "0.1.0";

    // Navigation properties

    /// <summary>
    /// Collection des joueurs présents dans ce monde.
    /// </summary>
    public ICollection<Player> Players { get; set; } = new List<Player>();

    // Business logic

    /// <summary>
    /// Indique si le monde est infini (génération procédurale illimitée).
    /// </summary>
    public bool IsInfinite => WorldSizeX == 0 || WorldSizeY == 0;

    /// <summary>
    /// Nombre de joueurs actuellement dans le monde.
    /// </summary>
    public int PlayerCount => Players.Count;

    /// <summary>
    /// Convertit le temps de jeu en TimeSpan pour faciliter l'affichage.
    /// </summary>
    public TimeSpan GameTimeSpan => TimeSpan.FromMilliseconds(GameTime);

    /// <summary>
    /// Met à jour le temps de jeu et la date de dernière session.
    /// </summary>
    /// <param name="deltaMilliseconds">Temps écoulé en millisecondes</param>
    public void UpdateGameTime(long deltaMilliseconds)
    {
        GameTime += deltaMilliseconds;
        LastPlayed = DateTime.UtcNow;
    }

    /// <summary>
    /// Génère une seed aléatoire pour un nouveau monde.
    /// </summary>
    /// <returns>Seed aléatoire</returns>
    public static long GenerateRandomSeed()
    {
        return Random.Shared.NextInt64();
    }
}
