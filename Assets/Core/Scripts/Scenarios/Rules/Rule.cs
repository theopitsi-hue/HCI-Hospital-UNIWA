using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Rule", menuName = "Scenario/Rule", order = 0)]
public class Rule : ScriptableObject
{
    public string id;
    public List<Condition> conditions = new();
    public List<Effect> effects = new();

    public bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in conditions)
        {
            if (!con.Evaluate(scenarioExecutor))
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyEffects(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in effects)
        {
            con.Apply(scenarioExecutor);
        }
    }
}