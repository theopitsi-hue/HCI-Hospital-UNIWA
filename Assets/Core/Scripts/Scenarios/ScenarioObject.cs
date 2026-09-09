
using System;
using System.Collections.Generic;
using System.Data.Common;
using AYellowpaper.SerializedCollections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

[SerializeField]
[CreateAssetMenu(fileName = "ScenarioObject", menuName = "Scenario/ScenarioObject", order = 0)]
public class ScenarioObject : ScriptableObject
{
    private const int _schema_version = 0;
    public ScenarioMeta scenarioMeta;
    public ScenarioState initialState;
    public SerializedDictionary<string, bool> ActiveHotspots = new();
    public GlobalRules globalRules;
    public LogInfo logInfo;
    public Nodemap nodemap;
    public Textmap textmap;

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }
    public static ScenarioObject FromJson(string json)
    {
        ScenarioObject scenario = ScriptableObject.CreateInstance<ScenarioObject>();
        //todo: add more validity checks
        JsonUtility.FromJsonOverwrite(json, scenario);

        return scenario;
    }
}

[Serializable]
public class Textmap
{
    public SerializedDictionary<int, string> dialogue = new();

}

[Serializable]
public class Nodemap
{
    public string entryNodeID = "";
    public SerializedDictionary<string, Node> nodes = new();

}

[Serializable]
public class ScenarioMeta
{
    public string id;
    public string title;
    public string description;
    public string estimatedDurationMinutes;
    public string difficulty;
    public string levelName;

    public List<String> learningGoals = new();
}

[Serializable]
public class ScenarioState
{
    public float timeElapsed = 0;
    public int currentScore = 0;

    public SerializedDictionary<string, bool> flags = new();

    public Vitals vitals;

    public ScenarioState(int timeElapsed, int currentScore, SerializedDictionary<string, bool> flags, Vitals vitals)
    {
        this.timeElapsed = timeElapsed;
        this.currentScore = currentScore;
        this.flags = flags;
        this.vitals = vitals;
    }

    public ScenarioState(ScenarioState other)
    {
        this.timeElapsed = other.timeElapsed;
        this.currentScore = other.currentScore;
        this.flags = other.flags;
        this.vitals = other.vitals;
    }
}

[Serializable]
public class Vitals
{
    public float heartRate;
    public float bloodOxygenSaturation;
    public float respiratoryRate;
    public float bloodPressureSystolic;   // large pressure (bp_big)
    public float bloodPressureDiastolic;  // small pressure (bp_small)
    public float bodyTemperature;
}

[Serializable]
public class GlobalRules
{
    public List<Rule> rules = new();
    [HideInInspector]
    public List<Rule> triggerDisabled = new();

    public void Disable(Rule item)
    {
        if (rules.Remove(item))
        {
            triggerDisabled.Add(item);
            Debug.Log("Disabled rule with id:" + item);
        }
    }
}

public class LogInfo
{
    public bool LoggingEnabled = true;
    public List<string> logEventTypes = new();
    public string exportFormat = "JSON";
}