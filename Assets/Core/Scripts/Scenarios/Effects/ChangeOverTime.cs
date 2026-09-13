
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ChangeOverTime : Effect
{
    public BlackboardKey flag;

    public float floatValueChangePerFrame = +1;
    public float minimum = 0;
    public float maximum = 100;

    public override void Apply(ScenarioExecutor exec)
    {
        FloatValue floatValue = (FloatValue)exec.blackboard.GetValue(flag);
        float val = (float)floatValue.GetValue();
        floatValue.SetValue(Math.Clamp(val + floatValueChangePerFrame * Time.deltaTime, minimum, maximum));
    }
}