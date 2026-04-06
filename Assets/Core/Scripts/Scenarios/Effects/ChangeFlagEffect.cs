[System.Serializable]
public class ChangeFlagEffect : Effect
{
    public BlackboardKey flag;

    public float floatValue = 0;
    public bool boolValue = false;

    public override void Apply(ScenarioExecutor exec)
    {
        GameManager.Instance.sceneExecutor.blackboard.GetValue(flag).SetValue(flag.type == BlackboardValueType.BOOL ? boolValue : floatValue);
    }
}