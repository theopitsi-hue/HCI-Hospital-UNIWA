using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using UnityEngine;

public static class GameLogger
{
    private static readonly ConcurrentQueue<string> LogQueue = new();

    private static readonly string LogFilePath =
        Path.Combine(Application.persistentDataPath, "game.log");

    private static Thread _writerThread;
    private static volatile bool _isRunning;
    private static readonly AutoResetEvent LogSignal = new(false);

    private static readonly object InitializationLock = new();

    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized)
            return;

        lock (InitializationLock)
        {
            if (_initialized)
                return;

            _isRunning = true;

            _writerThread = new Thread(WriteLogs)
            {
                IsBackground = true,
                Name = "GameLogger"
            };

            _writerThread.Start();

            _initialized = true;
        }
    }

    public static void Log(string message)
    {
        Write(LogLevel.Info, message);
    }

    public static void LogWarning(string message)
    {
        Write(LogLevel.Warning, message);
    }

    public static void LogError(string message)
    {
        Write(LogLevel.Error, message);
    }

    private static void Write(LogLevel level, string message)
    {
        Initialize();

        string timestamp =
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        string formattedMessage =
            $"[{timestamp}] [{level}] {message}";

        // Unity Console
        switch (level)
        {
            case LogLevel.Warning:
                Debug.LogWarning(message);
                break;

            case LogLevel.Error:
                Debug.LogError(message);
                break;

            default:
                Debug.Log(message);
                break;
        }

        // Queue for background file writing
        LogQueue.Enqueue(formattedMessage);
        LogSignal.Set();
    }

    private static void WriteLogs()
    {
        try
        {
            using StreamWriter writer = new(
                LogFilePath,
                append: true
            );

            while (_isRunning || !LogQueue.IsEmpty)
            {
                if (LogQueue.TryDequeue(out string message))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
                else
                {
                    LogSignal.WaitOne(100);
                }
            }

            // Write anything that arrived just before shutdown
            while (LogQueue.TryDequeue(out string message))
            {
                writer.WriteLine(message);
            }

            writer.Flush();
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"GameLogger failed to write log file: {exception}"
            );
        }
    }

    public static void Shutdown()
    {
        if (!_initialized)
            return;

        _isRunning = false;
        LogSignal.Set();

        if (_writerThread != null && _writerThread.IsAlive)
        {
            // Give the writer a few seconds to finish.
            _writerThread.Join(TimeSpan.FromSeconds(5));
        }

        _initialized = false;
    }

    private enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}