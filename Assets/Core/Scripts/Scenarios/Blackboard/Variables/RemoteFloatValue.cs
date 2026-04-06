using System;

/// <summary>
/// For float values that use getters/setters to change or get the value from a script.
/// </summary>
public class RemoteFloatValue : FloatValue
{
    private readonly Func<float> getter;
    private readonly Action<float> setter;

    public RemoteFloatValue(Func<float> getter, Action<float> setter)
    {
        this.getter = getter;
        this.setter = setter;
    }

    public override object GetValue() => getter();
    public override void SetValue(object i) => setter((float)i);
}