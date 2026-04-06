using System;

/// <summary>
/// For bool values that use getters/setters to change or get the value from a script.
/// </summary>
public class RemoteBoolVariable : BoolValue
{
    private readonly Func<bool> getter;
    private readonly Action<bool> setter;

    public RemoteBoolVariable(Func<bool> getter, Action<bool> setter)
    {
        this.getter = getter;
    }

    public override object GetValue() => getter();
    public override void SetValue(object i) => setter((bool)i);
}