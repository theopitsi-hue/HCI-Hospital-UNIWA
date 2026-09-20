
using UnityEngine;

[System.Serializable]
public class RecordActionEffect : Effect
{
    public string actionId;
    public string actionLabel;

    public override void Apply(ScenarioExecutor exec)
    {
        exec.AddActionToRecord(actionId, actionLabel);
    }
}