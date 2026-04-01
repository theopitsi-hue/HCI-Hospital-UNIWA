
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ChangeFlagEffect", menuName = "Scenario/ChangeFlagEffect", order = 0)]
public class ChangeFlagEffect : Effect
{

    [Space]
    [Header("Flag Change")]
    public string flagName;
    public bool value;

    [Header("Settings")]
    public bool flipOnConditionFails;

    public override void Apply(ScenarioExecutor exec)
    {
        exec.SetFlag(flagName, value);
    }

    public override void ApplyFailed(ScenarioExecutor exec)
    {
        if (flipOnConditionFails)
        {
            exec.SetFlag(flagName, !value);
        }
    }
}
