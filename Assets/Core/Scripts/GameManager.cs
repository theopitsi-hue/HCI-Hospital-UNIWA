using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ScenarioExecutor sceneExecutor;
    public UIManager uiManager;
    public Camera mainMenuCamera;
    public PlayerScenarioData playerData;
    public ScenarioLoader loader;

    public string loadedLevelScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        loader.LoadAllScenarios();
        mainMenuCamera.gameObject.SetActive(true);
        DontDestroyOnLoad(gameObject);
        uiManager.Initialize();
        uiManager.ActivateOnly(UIManager.UIType.MainMenu);
    }

    public void StartLevel(string levelName, ScenarioObject scenarioObject)
    {
        mainMenuCamera.gameObject.SetActive(false);
        print("Entering level: " + levelName);
        loadedLevelScene = levelName;
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);

        uiManager.ActivateOnly(UIManager.UIType.HUD);

        sceneExecutor.BeginScenario(scenarioObject);
    }

    public void QuitLevel()
    {
        mainMenuCamera.gameObject.SetActive(true);
        if (loadedLevelScene == null)
        {
            print("No scene loaded!");
            return;
        }

        print("Quitting level: " + loadedLevelScene);

        uiManager.ActivateOnly(UIManager.UIType.MainMenu);
        SceneManager.UnloadSceneAsync(loadedLevelScene);
        loadedLevelScene = null;
        playerData.Clear();
    }
}