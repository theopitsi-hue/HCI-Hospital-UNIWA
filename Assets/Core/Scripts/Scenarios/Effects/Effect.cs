
using System;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    [Header("Effect")]
    [Tooltip("Run this effect only when the condition fails.")]
    public bool runOnFail;

    public virtual void Apply(ScenarioExecutor exec)
    {

    }

    //called when the condition fails
    public virtual void ApplyFailed(ScenarioExecutor exec)
    {

    }
}
