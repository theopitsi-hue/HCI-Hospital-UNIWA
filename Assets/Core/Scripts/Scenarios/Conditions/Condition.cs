using UnityEngine;

[System.Serializable]
public class Condition : ScriptableObject
{
    [Header("Condition")]
    public string id;

    /// <returns>True if this condition is met.</returns>
    public virtual bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        return false;
    }
}
