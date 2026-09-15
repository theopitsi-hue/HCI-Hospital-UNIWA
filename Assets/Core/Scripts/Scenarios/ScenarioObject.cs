
using System;
using System.Collections.Generic;
using System.Data.Common;
using AYellowpaper.SerializedCollections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using System.Text;

[SerializeField]
[CreateAssetMenu(fileName = "ScenarioObject", menuName = "Scenario/ScenarioObject", order = 0)]
public class ScenarioObject : ScriptableObject
{
    private const int _schema_version = 0;
    public ScenarioMeta scenarioMeta;
    public ScenarioState initialState;
    //initial active hotspots
    public SerializedDictionary<string, bool> ActiveHotspots = new();
    public RuleManager globalRules;
    public LogInfo logInfo;
    //tree shaped diagram that handles transition logic
    public Nodemap nodemap;

    //static scenario data that gets referenced elsewhere
    //like documentation gates and dialogue/options
    public ScenarioStaticData staticData;

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
public class ScenarioStaticData
{
    [SerializeField]
    public SerializedDictionary<string, DocumentationGate> documentationGates;
    public Textmap textmap;
}

[Serializable]
public class DocumentationGate
{
    public string name;
    public List<BlackboardKey> requiredFields;

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"Gate(\'{name}\',");
        foreach (var item in requiredFields)
        {
            sb.Append(item.name);
            sb.Append(",");
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(")");
        return sb.ToString();
    }
}

[Serializable]
public class Textmap
{
    //todo embelish this
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

    public SerializedDictionary<string, bool> initialBoolKeys = new();
    public SerializedDictionary<string, float> initialNumberKeys = new();

    public Vitals vitals;

    public DocumentationGate activeDocumentationGate;
    private HashSet<string> completedGates = new();

    public void MarkGateCompleted(DocumentationGate gate)
    {
        Debug.Log("Completed documentation gate: " + gate);
        completedGates.Add(gate.name);
    }

    public bool IsGateCompleted(DocumentationGate gate)
    {
        return completedGates.Contains(gate.name);
    }


    public ScenarioState(int timeElapsed, int currentScore, SerializedDictionary<string, bool> flags, Vitals vitals)
    {
        this.timeElapsed = timeElapsed;
        this.currentScore = currentScore;
        this.initialBoolKeys = flags;
        this.vitals = vitals;
    }

    public ScenarioState(ScenarioState other)
    {
        this.timeElapsed = other.timeElapsed;
        this.currentScore = other.currentScore;
        this.initialBoolKeys = other.initialBoolKeys;
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
    public int skinType;
    public float breathRate;
    public float oxygenTankFuel;
}

[Serializable]
public class RuleManager
{
    public List<Rule> rules = new();
    [HideInInspector]
    public List<Rule> triggerDisabled = new();

    public void EvaluateAll(ScenarioExecutor exec)
    {
        for (int i = rules.Count - 1; i >= 0; i--)
        {
            var item = rules[i];

            if (item.Evaluate(exec))
            {
                item.ApplyPassEffects(exec);
                if (item.TriggerOnce)
                {
                    Disable(item);
                }
            }
            else
            {
                item.ApplyFailEffects(exec);
            }
        }
    }

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