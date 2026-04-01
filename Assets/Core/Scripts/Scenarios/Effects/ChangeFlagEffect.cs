
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ChangeFlagEffect", menuName = "Scenario/ChangeFlagEffect", order = 0)]
public class ChangeFlagEffect : Effect
{
    public string flagName;
    public bool value;

    public override void Apply(ScenarioExecutor exec)
    {
        exec.SetFlag(flagName, value);
    }
}
