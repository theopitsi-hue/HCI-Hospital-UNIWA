[System.Serializable]
public class GoToNodeEffect : Effect
{
    public string nodeID = "";

    public override void Apply(ScenarioExecutor exec)
    {
        exec.nodeManager.TryTransition(exec, nodeID);
    }
}