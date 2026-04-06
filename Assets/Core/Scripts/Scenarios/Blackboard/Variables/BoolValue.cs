using System;

/// <summary>
/// For bool values that live only in the blackboard.
/// </summary>
public class BoolValue : BlackboardValue
{
    private bool value;
    public BoolValue(bool value)
    {
        this.value = value;
    }
    public BoolValue()
    {
    }

    public override object GetValue() => value;
    public override void SetValue(object i) => value = ((bool)i);
}