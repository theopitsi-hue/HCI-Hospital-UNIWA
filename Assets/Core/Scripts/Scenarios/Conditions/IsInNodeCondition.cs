using System;
using UnityEngine;

[Serializable]
public class IsInNodeCondition : Condition
{
    public string nodeName;
    public override bool Evaluate(ScenarioExecutor exec)
    {
        return exec.nodeManager.IsInNode(nodeName);
    }
}