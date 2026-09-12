using System;
using UnityEngine;

[Serializable]
public class IsDocumentationGateCompleteCondition : Condition
{
    public string gateName;
    public override bool Evaluate(ScenarioExecutor exec)
    {
        if (exec.TryGetDocumentationGate(gateName, out var gate))
        {
            return exec.IsGateCompleted(gate);
        }
        return false;
    }
}