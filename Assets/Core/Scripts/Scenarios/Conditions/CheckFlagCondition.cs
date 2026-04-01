using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CheckFlagCondition", menuName = "Scenario/CheckFlagCondition", order = 0)]
public class CheckFlagCondition : Condition
{
    public string flagName;
    public bool requiredValue;

    public override bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        Debug.Log("Checking: " + flagName + " result:" + scenarioExecutor.GetFlag(flagName));
        return scenarioExecutor.GetFlag(flagName) && scenarioExecutor.GetFlag(flagName) == requiredValue;
    }
}
