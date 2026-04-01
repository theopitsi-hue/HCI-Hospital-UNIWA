using UnityEngine;

[System.Serializable]
public class Condition : ScriptableObject
{
    public string id;

    /// <returns>True if this condition is met.</returns>
    public virtual bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        return false;
    }
}
