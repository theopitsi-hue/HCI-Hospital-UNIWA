using System;

/// <summary>
/// For float values that live only in the blackboard.
/// </summary>
public class FloatValue : BlackboardValue
{
    private float value;
    public FloatValue() { }

    public override object GetValue() => value;
    public override void SetValue(object i) => value = ((float)i);
}