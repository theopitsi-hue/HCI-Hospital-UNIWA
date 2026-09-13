
using UnityEngine;

[System.Serializable]
public class ChangeFlagBasicEffect : Effect
{
    public string flag;
    public bool boolValue = false;

    public override void Apply(ScenarioExecutor exec)
    {
        Debug.Log("Changed flag:" + flag + " to value: " + boolValue);
        GameManager.Instance.sceneExecutor.blackboard.GetValue(flag).SetValue(boolValue);
    }
}