using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class Blackboard
{
    [HideInInspector]
    public SerializedDictionary<string, BlackboardValue> BlackboardVariables = new();


    public void RegisterValue(string name, BlackboardValue bal)
    {
        Debug.Log("Registering:" + name + " | " + bal.GetType().Name + "");
        BlackboardVariables[name] = bal;
    }

    public BlackboardValue GetValue(string name)
    {
        if (BlackboardVariables.TryGetValue(name, out var b))
        {
            return b;
        }
        Debug.LogError("Blackboard variable doesnt exist: '" + name + "', Have you registered it?");
        return null;
    }

    public BlackboardValue GetValue(BlackboardKey name)
    {
        return GetValue(name.name);
    }

    public void Clear()
    {
        BlackboardVariables.Clear();
    }
}