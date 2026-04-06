using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public class CheckFlagCondition : Condition
{
    [Header("Check")]
    public ComparisonMode comparisonMode = ComparisonMode.FLAT;
    public BlackboardKey left;
    public BlackboardKey right;

    public float rightNum;
    public bool rightBool;

    public NumberComparisonType numberComparisonType;
    public BooleanComparisonType booleanComparisonType;

    public override bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        if (left.type == BlackboardValueType.BOOL && left.TryGetValue(out var valb))
        {
            switch (comparisonMode)
            {
                case ComparisonMode.FLAT:
                    {
                        return CompareBool((bool)valb.GetValue(), rightBool);
                    }
                case ComparisonMode.VARIABLE:
                    {
                        if (right.type != BlackboardValueType.BOOL)
                        {
                            throw new System.Exception("Mismatched conditions");
                        }

                        if (right.TryGetValue(out var valbr))
                        {
                            return CompareBool((bool)valb.GetValue(), (bool)valbr.GetValue());
                        }
                        else
                        {
                            Debug.LogError("Key doesnt exist:" + right.name);
                        }
                        return false;
                    }
            }
        }
        if (left.type == BlackboardValueType.FLOAT && left.TryGetValue(out var val))
        {
            switch (comparisonMode)
            {
                case ComparisonMode.FLAT:
                    {
                        return CompareNumber((float)val.GetValue(), rightNum);
                    }

                case ComparisonMode.VARIABLE:
                    {
                        if (right.type != BlackboardValueType.FLOAT)
                        {
                            throw new System.Exception("Mismatched conditions");
                        }
                        if (right.TryGetValue(out var valfr))
                        {
                            return CompareNumber((float)val.GetValue(), (float)valfr.GetValue());
                        }
                        else
                        {
                            Debug.LogError("Key doesnt exist:" + right.name);

                        }
                        return false;
                    }
            }
        }
        return false;
    }

    private bool CompareNumber(float left, float right)
    {
        switch (numberComparisonType)
        {
            case NumberComparisonType.Equal: return left == right;
            case NumberComparisonType.NotEqual: return left != right;
            case NumberComparisonType.Greater: return left > right;
            case NumberComparisonType.GreaterOrEqual: return left >= right;
            case NumberComparisonType.Less: return left < right;
            case NumberComparisonType.LessOrEqual: return left <= right;
            default: return false;
        }
    }

    private bool CompareBool(bool left, bool right)
    {
        switch (booleanComparisonType)
        {
            case BooleanComparisonType.Equal: return left == right;
            case BooleanComparisonType.NotEqual: return left != right;
            default: return false;
        }
    }

    public override string ToString()
    {
        if (left.TryGetValue(out var v))
        {
            return "Condition(Left:" + v + ") - " + Evaluate(null);
        }
        return "nope";
    }
}

[Serializable]
public enum ComparisonMode
{
    VARIABLE, FLAT
}

