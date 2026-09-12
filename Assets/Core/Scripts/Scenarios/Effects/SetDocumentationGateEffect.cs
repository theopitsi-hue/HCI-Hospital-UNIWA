using UnityEngine;

[System.Serializable]
public class SetDocumentationGateEffect : Effect
{
    public string documentationGateName;

    public override void Apply(ScenarioExecutor exec)
    {
        //try load the requested gate
        if (exec.TryGetDocumentationGate(documentationGateName, out var gate))
        {
            exec.SetActiveDocumentationGate(gate);
        }
    }
}