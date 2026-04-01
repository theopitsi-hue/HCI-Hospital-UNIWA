
using System;
using UnityEngine;

[System.Serializable]
public abstract class Effect : ScriptableObject
{

    [Header("Effect")]
    public string id;

    public virtual void Apply(ScenarioExecutor exec)
    {

    }

    //called when the condition fails
    public virtual void ApplyFailed(ScenarioExecutor exec)
    {

    }
}
