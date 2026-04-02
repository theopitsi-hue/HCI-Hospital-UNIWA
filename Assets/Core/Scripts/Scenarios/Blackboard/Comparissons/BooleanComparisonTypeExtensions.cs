using System.Collections.Generic;

public static class BooleanComparisonTypeExtensions
{
    private static readonly Dictionary<string, BooleanComparisonType> fromJson =
        new()
        {
            ["equal"] = BooleanComparisonType.Equal,
            ["not_equal"] = BooleanComparisonType.NotEqual
        };

    public static string ToJsonOp(this BooleanComparisonType type)
    {

        return type switch
        {
            BooleanComparisonType.Equal => "equal",
            BooleanComparisonType.NotEqual => "not_equal",
            _ => "unknown"
        };
    }


    public static BooleanComparisonType FromJsonOp(string op)
    {
        if (fromJson.TryGetValue(op, out var value))
            return value;

        throw new System.Exception($"Unknown bool comparison op: {op}");
    }
}


public static class NumberComparisonTypeExtensions
{
    private static readonly Dictionary<string, NumberComparisonType> fromJson =
        new()
        {
            ["equal"] = NumberComparisonType.Equal,
            ["not_equal"] = NumberComparisonType.NotEqual,
            ["greater_than"] = NumberComparisonType.Greater,
            ["greater_or_equal"] = NumberComparisonType.GreaterOrEqual,
            ["less_than"] = NumberComparisonType.Less,
            ["less_or_equal"] = NumberComparisonType.LessOrEqual
        };

    public static string ToJsonOp(this NumberComparisonType type)
    {
        return type switch
        {
            NumberComparisonType.Equal => "equal",
            NumberComparisonType.NotEqual => "not_equal",
            NumberComparisonType.Greater => "greater_than",
            NumberComparisonType.GreaterOrEqual => "greater_or_equal",
            NumberComparisonType.Less => "less_than",
            NumberComparisonType.LessOrEqual => "less_or_equal",
            _ => "unknown"
        };
    }

    public static NumberComparisonType FromJsonOp(string op)
    {
        if (fromJson.TryGetValue(op, out var value))
            return value;

        throw new System.Exception($"Unknown number comparison op: {op}");
    }

}