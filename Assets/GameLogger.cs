using System;
using System.IO;
using UnityEngine;

public static class GameLogger
{
    private static readonly string LogFilePath =
        Path.Combine(Application.persistentDataPath, "game.log");

    public static void Log(string message)
    {
        string timestampedMessage =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        // Log to Unity Console
        Debug.Log(timestampedMessage);

        // Log to file
        try
        {
            File.AppendAllText(
                LogFilePath,
                timestampedMessage + Environment.NewLine
            );
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to log file: {e}");
        }
    }

    public static void LogWarning(string message)
    {
        string timestampedMessage =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        Debug.LogWarning(timestampedMessage);

        try
        {
            File.AppendAllText(
                LogFilePath,
                "[WARNING] " + timestampedMessage + Environment.NewLine
            );
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to log file: {e}");
        }
    }

    public static void LogError(string message)
    {
        string timestampedMessage =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

        Debug.LogError(timestampedMessage);

        try
        {
            File.AppendAllText(
                LogFilePath,
                "[ERROR] " + timestampedMessage + Environment.NewLine
            );
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to log file: {e}");
        }
    }
}