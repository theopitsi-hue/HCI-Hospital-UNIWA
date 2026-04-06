using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Condition
{
    /// <returns>True if this condition is met.</returns>
    public virtual bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        return false;
    }
}
