using System;
using UnityEngine;

[Serializable]
public class WaitSecondsCondition : Condition
{

    public float waitTime = 1f;
    private float timeElapsed;
    private float lastCheck = -1;

    public override bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        if (lastCheck < 0)
        {
            lastCheck = Time.time;
        }
        timeElapsed += Time.time - lastCheck;
        lastCheck = Time.time;
        if (timeElapsed >= waitTime)
        {
            lastCheck = -1;
            timeElapsed = 0;
            return true;
        }
        return false;
    }
}