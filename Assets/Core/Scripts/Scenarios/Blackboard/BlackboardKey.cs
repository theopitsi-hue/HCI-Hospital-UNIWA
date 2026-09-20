
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackboardKey", menuName = "Scenario/BlackboardKey", order = 0)]
public class BlackboardKey : ScriptableObject
{
    [SerializeField]
    public BlackboardValueType type;
    public static BlackboardKey Find(string key)
    {
        return Resources.Load<BlackboardKey>($"Core/ScriptableObjects/Keys/{key}");
    }

    public bool TryGetValue(out BlackboardValue val)
    {
        val = null;
        if (GameManager.Instance.sceneExecutor == null)
        {
            GameLogger.Log("SceneExecutor Missing");
            return false;
        }
        if (GameManager.Instance.sceneExecutor.blackboard == null)
        {
            GameLogger.Log("blackboard Missing");
            return false;
        }
        var cal = GameManager.Instance.sceneExecutor.blackboard.GetValue(name);
        if (cal == null)
        {
            GameLogger.Log("key Missing");
            return false;
        }
        val = GameManager.Instance.sceneExecutor.blackboard.GetValue(name);
        return true;
    }
}

public enum BlackboardValueType
{
    BOOL, FLOAT
}