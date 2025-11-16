namespace TinySurvivalWorld.Core.Enums;

/// <summary>
/// Types de tuiles du monde.
/// </summary>
public enum TileType : byte
{
    /// <summary>
    /// Eau profonde (océan, lac).
    /// </summary>
    DeepWater = 0,

    /// <summary>
    /// Eau peu profonde.
    /// </summary>
    ShallowWater = 1,

    /// <summary>
    /// Sable (plage, désert).
    /// </summary>
    Sand = 2,

    /// <summary>
    /// Herbe (plaine).
    /// </summary>
    Grass = 3,

    /// <summary>
    /// Terre (sol nu).
    /// </summary>
    Dirt = 4,

    /// <summary>
    /// Forêt dense.
    /// </summary>
    Forest = 5,

    /// <summary>
    /// Forêt clairsemée.
    /// </summary>
    SparseForest = 6,

    /// <summary>
    /// Colline/montagne basse.
    /// </summary>
    Hill = 7,

    /// <summary>
    /// Montagne.
    /// </summary>
    Mountain = 8,

    /// <summary>
    /// Pic de montagne enneigé.
    /// </summary>
    SnowPeak = 9,

    /// <summary>
    /// Marécage.
    /// </summary>
    Swamp = 10,

    /// <summary>
    /// Ruines (structure post-apocalyptique).
    /// </summary>
    Ruins = 11,

    /// <summary>
    /// Zone toxique/contaminée (déchets toxiques, pollution chimique).
    /// </summary>
    Toxic = 12
}
