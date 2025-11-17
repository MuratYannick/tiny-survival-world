using System;
using System.IO;

namespace TinySurvivalWorld.Game.Desktop.Utilities;

/// <summary>
/// Logger pour enregistrer les erreurs et événements dans un fichier.
/// </summary>
public static class GameLogger
{
    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TinySurvivalWorld",
        "game.log"
    );

    private static readonly object _lockObject = new object();

    static GameLogger()
    {
        try
        {
            // Créer le dossier de logs s'il n'existe pas
            var logDirectory = Path.GetDirectoryName(LogFilePath);
            if (logDirectory != null && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Écrire un header au démarrage
            lock (_lockObject)
            {
                File.AppendAllText(LogFilePath, $"\n\n=== SESSION DÉMARRÉE: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n");
            }
        }
        catch
        {
            // Si on ne peut pas créer le fichier de log, on continue sans
        }
    }

    /// <summary>
    /// Log une information générale.
    /// </summary>
    public static void Info(string message)
    {
        WriteLog("INFO", message);
    }

    /// <summary>
    /// Log un avertissement.
    /// </summary>
    public static void Warning(string message)
    {
        WriteLog("WARNING", message);
    }

    /// <summary>
    /// Log une erreur.
    /// </summary>
    public static void Error(string message, Exception? ex = null)
    {
        var fullMessage = ex != null
            ? $"{message}\nException: {ex.GetType().Name}\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}"
            : message;

        WriteLog("ERROR", fullMessage);
    }

    /// <summary>
    /// Obtient le chemin du fichier de log.
    /// </summary>
    public static string GetLogFilePath() => LogFilePath;

    private static void WriteLog(string level, string message)
    {
        try
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}\n";

            lock (_lockObject)
            {
                File.AppendAllText(LogFilePath, logEntry);
            }

            // Aussi écrire dans la console de debug
            System.Diagnostics.Debug.WriteLine(logEntry);
        }
        catch
        {
            // Si on ne peut pas écrire dans le log, on continue sans
        }
    }
}
