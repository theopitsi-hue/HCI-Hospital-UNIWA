using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CheckBBValue", menuName = "Scenario/CheckBBValue", order = 0)]
public class CheckBBValue : Condition
{
    [Header("Check")]
    public ComparisonMode comparisonMode = ComparisonMode.NUMBER;
    public BlackboardKey left;
    public BlackboardKey right;

    public float rightNum;
    public bool rightBool;

    public NumberComparisonType numberComparisonType;
    public BooleanComparisonType booleanComparisonType;

    public override bool Evaluate(ScenarioExecutor scenarioExecutor)
    {
        switch (comparisonMode)
        {
            case ComparisonMode.NUMBER:
                {
                    var lv = left.Get();
                    var rv = right.Get();

                    if (lv is not FloatValue fl || rv is not FloatValue fr)
                        throw new System.Exception("CheckBBValue: NUMBER mode requires both left and right to be FloatValue (Blackboard keys).");

                    float leftValue = (float)fl.GetValue();
                    float rightValue = (float)fr.GetValue();

                    return CompareNumber(leftValue, rightValue);
                }

            case ComparisonMode.BOOL:
                {
                    var lv = left.Get();
                    var rv = right.Get();

                    if (lv is not BoolValue bl || rv is not BoolValue br)
                        throw new System.Exception("CheckBBValue: BOOL mode requires both left and right to be BoolValue (Blackboard keys).");

                    bool leftValue = (bool)bl.GetValue();
                    bool rightValue = (bool)br.GetValue();

                    return CompareBool(leftValue, rightValue);
                }

            case ComparisonMode.FLAT_NUMBER:
                {
                    var lv = left.Get();

                    if (lv is not FloatValue fl)
                        throw new System.Exception("CheckBBValue: FLAT_NUMBER mode requires left to be FloatValue.");

                    float leftValue = (float)fl.GetValue();
                    float rightValue = rightNum;

                    return CompareNumber(leftValue, rightValue);
                }

            case ComparisonMode.FLAT_BOOL:
                {
                    var lv = left.Get();

                    if (lv is not BoolValue bl)
                        throw new System.Exception("CheckBBValue: FLAT_BOOL mode requires left to be BoolValue.");

                    bool leftValue = (bool)bl.GetValue();
                    bool rightValue = rightBool;

                    return CompareBool(leftValue, rightValue);
                }

            default:
                throw new System.Exception($"CheckBBValue: Unknown comparison mode {comparisonMode}");
        }
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

    public string ToJson()
    {
        string key = left.name; // or your BlackboardKey identifier

        JObject condition = new JObject();

        JObject inner = new JObject();

        switch (comparisonMode)
        {
            case ComparisonMode.NUMBER:
                {
                    inner[numberComparisonType.ToJsonOp()] = left.name;
                    break;
                }

            case ComparisonMode.FLAT_NUMBER:
                {
                    inner[numberComparisonType.ToJsonOp()] = rightNum;
                    break;
                }

            case ComparisonMode.BOOL:
                {
                    inner[booleanComparisonType.ToJsonOp()] = left.name;
                    break;
                }

            case ComparisonMode.FLAT_BOOL:
                {
                    inner[booleanComparisonType.ToJsonOp()] = rightBool;
                    break;
                }

            default:
                throw new System.Exception("Invalid comparison mode");
        }

        condition[key] = inner;

        JObject root = new JObject
        {
            ["condition"] = condition
        };

        return root.ToString(Formatting.Indented);
    }

    //todo: make sure the blackboard type match the condition type (like flat number -> floatValue)
    public static CheckBBValue FromJson(string json)
    {
        JObject root = JObject.Parse(json);

        JObject condition = (JObject)root["condition"];

        var firstProp = condition.Properties().First();

        string key = firstProp.Name;
        JObject inner = (JObject)firstProp.Value;

        var opProp = inner.Properties().First();

        string op = opProp.Name;
        JToken value = opProp.Value;

        var result = ScriptableObject.CreateInstance<CheckBBValue>();

        result.left = BlackboardKey.Get(key);

        if (value.Type == JTokenType.Float || value.Type == JTokenType.Integer)
        {
            result.comparisonMode = ComparisonMode.FLAT_NUMBER;
            result.numberComparisonType = NumberComparisonTypeExtensions.FromJsonOp(op);
            result.rightNum = value.Value<float>();
        }
        else if (value.Type == JTokenType.Boolean)
        {
            result.comparisonMode = ComparisonMode.FLAT_BOOL;
            result.booleanComparisonType = BooleanComparisonTypeExtensions.FromJsonOp(op);
            result.rightBool = value.Value<bool>();
        }
        else
        {
            throw new System.Exception("Unsupported JSON value type");
        }

        return result;
    }
}

public enum ComparisonMode
{
    NUMBER,
    BOOL,
    FLAT_NUMBER,
    FLAT_BOOL
}
