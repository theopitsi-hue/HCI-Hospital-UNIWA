
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DeltaBlackboardFloatEffect : Effect
{
    public BlackboardKey flag;

    public float floatValueChange = +10;
    public float timeUntilApex = 1;

    public override void Apply(ScenarioExecutor exec)
    {
        //request the change from the value manager
        exec.ChangeValueOverTime(flag, floatValueChange, timeUntilApex);
    }
}