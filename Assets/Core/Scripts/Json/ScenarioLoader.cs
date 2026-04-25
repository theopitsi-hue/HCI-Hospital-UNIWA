using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScenarioLoader : MonoBehaviour
{
    public string fileName = "scenario.json";
    // Start is called before the first frame update
    void Start()
    {
        LoadScenario();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScenario()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            ScenarioWrapper scenarioData = JsonUtility.FromJson<ScenarioWrapper>(json);

            Debug.Log("Title: " + scenarioData.scenario.title);
            Debug.Log("Duration: " + scenarioData.scenario.estimated_duration_minutes);
            for (int i=0; i< scenarioData.scenario.learning_goals.Length; i++)
                Debug.Log("Objective" + (i+1) +": " + scenarioData.scenario.learning_goals[i]);
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }
}
