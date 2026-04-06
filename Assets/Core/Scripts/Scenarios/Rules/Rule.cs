using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Rule", menuName = "Scenario/Rule", order = 0)]
public class Rule : ScriptableObject
{
    public string id;
    public List<Condition> conditions = new();
    [SerializeReference, SubclassSelector]
    public List<Effect> effects = new();

    public bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in conditions)
        {
            if (con && !con.Evaluate(scenarioExecutor))
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyPassEffects(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in effects)
        {
            if (con != null)
                con.Apply(scenarioExecutor);
        }
    }

    public void ApplyFailEffects(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in effects)
        {
            if (con != null)
                con.ApplyFailed(scenarioExecutor);
        }
    }
}