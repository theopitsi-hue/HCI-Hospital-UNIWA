using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Rule
{
    public string id;

    [Tooltip("Makes this rule trigger ONLY once, no matter if the conditions are met again. NEEDS the rule to have an ID in order to work properly.")]
    [SerializeField]
    private bool triggerOnce = true;

    public bool TriggerOnce => triggerOnce && id != null && id != "";

    [SerializeReference, SubclassSelector]
    public List<Condition> conditions = new();
    [SerializeReference, SubclassSelector]
    public List<Effect> effects = new();

    public bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in conditions)
        {
            if (!con.Evaluate(scenarioExecutor))
            {
                //Debug.Log("Rule condition FAILED: " + id);
                return false;
            }
        }
        //        Debug.Log("Rule condition PASSED: " + id);
        return true;
    }

    public void ApplyPassEffects(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in effects)
        {
            if (con != null)
            {
                con.Apply(scenarioExecutor);
            }
        }

        // Debug.Log("Applied rule effects: " + id);
    }

    public void ApplyFailEffects(ScenarioExecutor scenarioExecutor)
    {
        foreach (var con in effects)
        {
            if (con != null)
            {
                con.ApplyFailed(scenarioExecutor);
            }
        }
    }
    public override string ToString()
    {
        return "Rule(id:" + id + ")";
    }
}