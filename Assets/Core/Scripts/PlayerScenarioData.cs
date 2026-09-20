using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScenarioData : MonoBehaviour
{
    public List<BlackboardKey> knownValues = new List<BlackboardKey>();

    public void AddKnownValue(BlackboardKey key)
    {
        if (!knownValues.Contains(key))
        {
            knownValues.Add(key);
        }
    }

    public void Clear()
    {
        Debug.Log("Cleared player knowledge!");
        knownValues.Clear();
    }

    public bool KnowsValue(BlackboardKey key)
    {
        return knownValues.Contains(key);
    }
}
