using System;

public class FloatValue : BlackboardValue
{
    private Func<float> getter;

    public FloatValue(Func<float> getter)
    {
        this.getter = getter;
    }

    public override object GetValue() => getter();
}