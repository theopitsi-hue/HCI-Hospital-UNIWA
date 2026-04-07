using System;
using System.Data.Common;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class NodeManager
{
    private SerializedDictionary<string, Node> nodeMap = new();
    private Node activeNode;

    public void LoadScenarioNodes(ScenarioExecutor exec, Nodemap nodemap)
    {
        foreach (var item in nodemap.nodes)
        {
            nodeMap.Add(item.Key, item.Value);
        }

        TryTransition(exec, nodemap.entryNodeID);
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
            Debug.LogError("NodeManager:GoToNode - Next node '" + newNode + "' is null.");
            return;
        }
        if (activeNode != null && newNode.id == activeNode.id)
        {
            return;
        }
        activeNode?.OnExit(exec);
        activeNode = newNode;
        activeNode?.OnEnter(exec);

        Debug.Log("Entered node:" + newNode.id);
    }

    public bool Exists(String id)
    {
        return nodeMap.ContainsKey(id);
    }

    public void TryTransition(ScenarioExecutor exec, string nodeID)
    {
        if (Exists(nodeID) && nodeMap.TryGetValue(nodeID, out var node))
        {
            GoToNode(exec, node);
        }
    }
}