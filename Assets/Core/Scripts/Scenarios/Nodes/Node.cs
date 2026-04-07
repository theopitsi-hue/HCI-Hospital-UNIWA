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

    }

    public void OnExit(ScenarioExecutor exec)
    {

    }

    public void Update(ScenarioExecutor exec)
    {

    }
}