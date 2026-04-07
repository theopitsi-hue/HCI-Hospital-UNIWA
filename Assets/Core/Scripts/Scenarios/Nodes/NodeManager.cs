using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class NodeManager
{
    private SerializedDictionary<string, Node> nodeMap = new();
    private Node activeNode;

    public void LoadScenarioNodes(SerializedDictionary<string, Node> nodeMap)
    {

    }

    public void Update(ScenarioExecutor exec)
    {
        if (activeNode == null) return;
        activeNode.Update(exec);
    }

    private void GoToNode(ScenarioExecutor exec, Node newNode)
    {
        if (newNode == null)
        {
            Debug.LogError("NodeManager:GoToNode - Next node is null.");
            return;
        }
        activeNode.OnExit(exec);
        activeNode = newNode;
        activeNode.OnEnter(exec);
    }

    public bool Exists(String id)
    {
        return nodeMap.ContainsKey(id);
    }
}