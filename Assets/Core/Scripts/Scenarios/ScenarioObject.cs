
using System;
using System.Collections.Generic;
using System.Data.Common;
using AYellowpaper.SerializedCollections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;

[SerializeField]
[CreateAssetMenu(fileName = "ScenarioObject", menuName = "ScenarioObject", order = 0)]
public class ScenarioObject : ScriptableObject
{
    private const int schema_version = 0;
    public ScenarioMeta scenario_meta;
    public InitialState initial_state;
    public GlobalRules global_rules;
}


[Serializable]
public class ScenarioMeta
{
    public string id;
    public string title;
    public string description;
    [Label("Estimated Duration Minutes")]
    public string estimated_duration_minutes;
    public string difficulty;

    [Label("Learning Goals")]
    public List<String> learning_goals = new();
}

[Serializable]
public class InitialState
{
    [Label("Time Elapsed")]
    public int time_elapsed = 0;
    [Label("Current Score")]
    public int current_score = 0;

    public SerializedDictionary<string, bool> flags = new();

    public Vitals vitals;

}

[Serializable]
public class Vitals
{
    public int hr;
    public int spo2;
    public int rr;
    public int bp_big; //megalh piesh
    public int bp_small; //mikrh piesh
    public int temp;
}

[Serializable]
public class GlobalRules
{
    public List<Rule> rules = new();
}

[Serializable]
public class Rule
{
    public string id;
    public List<Condition> conditions = new();
    public List<Effect> effects = new();

    public bool Evaluate()
    {
        foreach (var con in conditions)
        {
            if (!con.Evaluate())
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyEffects()
    {
        foreach (var con in effects)
        {
            con.Apply();
        }
    }
}

[Serializable]
public class Condition
{
    public string id;

    /// <returns>True if this condition is met.</returns>
    public virtual bool Evaluate()
    {
        return false;
    }
}

[Serializable]
public class Effect
{
    public string id;
    public string type;

    public virtual void Apply()
    {

    }

}