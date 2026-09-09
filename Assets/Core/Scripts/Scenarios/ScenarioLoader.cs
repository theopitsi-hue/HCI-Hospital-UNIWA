using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioLoader : MonoBehaviour
{
    private string LoadPath;

    public List<ScenarioObject> editorScenarios = new List<ScenarioObject>();

    [HideInInspector]
    public List<ScenarioObject> loadedScenarios = new List<ScenarioObject>();

    public Dictionary<string, ScenarioObject> loadedScenariosDict = new Dictionary<string, ScenarioObject>();

    public UnityEvent onScenariosLoaded = new UnityEvent();
    private bool _isLoading;

    public void ClearLoaded()
    {
        loadedScenarios.Clear();
        loadedScenariosDict.Clear();
    }

    public void LoadAllScenarios()
    {
        _isLoading = true;
        LoadPath = Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
           "HCIScenarios"
       );
        loadedScenarios.Clear();

        if (!Directory.Exists(LoadPath))
        {
            Directory.CreateDirectory(LoadPath);
            Debug.Log("Created scenario folder: " + LoadPath);
            return;
        }

        Debug.Log("Searching for Json: " + LoadPath);

        string[] files = Directory.GetFiles(LoadPath, "*.json");

        Debug.Log("Found: " + files.Length + " scenario files");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);

            ScenarioObject scenario = ScenarioObject.FromJson(json);
            scenario.scenarioMeta.title += " (JSON)";
            AddScenario(scenario);
            Debug.Log("Loaded: " + Path.GetFileName(file));
        }


        foreach (var scenario in editorScenarios)
        {
            var runtime = Instantiate(scenario);
            runtime.scenarioMeta.title += " (EDITOR)";
            runtime.scenarioMeta.id += "_e";
            AddScenario(runtime);
        }

        onScenariosLoaded?.Invoke();
        _isLoading = false;
    }

    public void AddScenario(ScenarioObject scenario)
    {
        if (scenario.scenarioMeta.id == null || scenario.scenarioMeta.id == "")
        {
            Debug.LogError("Scenario has no id, therefore cannot be loaded.");
            return;
            //todo: add more validity checks
        }


        loadedScenariosDict.Add(scenario.scenarioMeta.id, scenario);
        loadedScenarios.Add(scenario);

    }

    public void Reload()
    {
        if (_isLoading) return;
        ClearLoaded();
        LoadAllScenarios();
    }
}