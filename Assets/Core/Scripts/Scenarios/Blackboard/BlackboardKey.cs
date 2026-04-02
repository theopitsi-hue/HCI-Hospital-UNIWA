
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackboardKey", menuName = "Scenario/BlackboardKey", order = 0)]
public class BlackboardKey : ScriptableObject
{
    public static BlackboardKey Get(string key)
    {
        return Resources.Load<BlackboardKey>($"Core/ScriptableObjects/Keys/{key}");
    }

    public BlackboardValue Get()
    {
        if (GameManager.Instance.sceneExecutor == null)
        {
            Debug.Log("SceneExecutor Missing");
        }
        if (GameManager.Instance.sceneExecutor.blackboard == null)
        {
            Debug.Log("blackboard Missing");
        }
        var cal = GameManager.Instance.sceneExecutor.blackboard.GetValue(name);
        if (cal == null)
        {
            Debug.Log("key Missing");
        }
        return GameManager.Instance.sceneExecutor.blackboard.GetValue(name);
    }
}