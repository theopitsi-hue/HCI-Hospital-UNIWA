using UnityEngine;

public class GameLoggerManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameLogger.Initialize();
    }

    private void OnApplicationQuit()
    {
        GameLogger.Shutdown();
    }
}