using TMPro;
using UnityEngine;

public class ThermometerUI : MachineUI
{
    [SerializeField] private TMP_Text temperatureText;

    public override void Setup(Machine machine)
    {
        base.Setup(machine);
        Debug.Log(machine.name);
    }

    protected override void DoUITick()
    {
        if (!keys[0].TryGetValue(out var v))
        {
            return;
        }

        float val = (float)v.GetValue();
        temperatureText.text = val.ToString("F1");
    }
}