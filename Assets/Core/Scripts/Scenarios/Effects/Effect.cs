
using UnityEngine;

[System.Serializable]
public abstract class Effect : ScriptableObject
{
    public string id;
    public string type;

    public virtual void Apply(ScenarioExecutor exec)
    {

    }
}
