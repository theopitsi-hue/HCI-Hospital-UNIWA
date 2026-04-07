using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public string id;

    [SerializeReference, SubclassSelector]
    [Tooltip("Effects to trigger when entering this node.")]
    public List<Effect> onEnterEffects = new();

    [SerializeReference, SubclassSelector]
    [Tooltip("Effects to trigger when leaving this node.")]
    public List<Effect> onExitEffects = new();

    [Tooltip("Conditions for transitioning off to the next nodes.")]
    public List<Rule> nodeTransitionRules = new();


    public void OnEnter(ScenarioExecutor exec)
    {
        foreach (var item in onEnterEffects)
        {
            item.Apply(exec);
        }
    }

    public void OnExit(ScenarioExecutor exec)
    {
        foreach (var item in onExitEffects)
        {
            item.Apply(exec);
        }
    }

    public void Update(ScenarioExecutor exec)
    {
        foreach (var item in nodeTransitionRules)
        {
            if (item.Evaluate(exec))
            {
                item.ApplyPassEffects(exec);
            }
        }
    }

    public override string ToString()
    {
        return "Node(id: " + id + ")";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return id.Equals((obj as Node).id);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}