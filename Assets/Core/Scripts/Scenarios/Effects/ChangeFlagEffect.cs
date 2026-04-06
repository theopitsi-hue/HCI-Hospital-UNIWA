
using UnityEngine;

[System.Serializable]
public class ChangeFlagEffect : Effect
{
    public BlackboardKey flag;

    public float floatValue = 0;
    public bool boolValue = false;

    public override void Apply(ScenarioExecutor exec)
    {
        Debug.Log("Changed flag:" + flag.name + " to value: " + (flag.type == BlackboardValueType.BOOL ? boolValue : floatValue));
        GameManager.Instance.sceneExecutor.blackboard.GetValue(flag).SetValue(flag.type == BlackboardValueType.BOOL ? boolValue : floatValue);
    }
}