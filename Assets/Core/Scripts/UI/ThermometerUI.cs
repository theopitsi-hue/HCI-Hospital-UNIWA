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


    public void OnButtonClick()
    {
        if (!GameManager.Instance.playerData.KnowsValue(keys[0]))
        {
            GameManager.Instance.playerData.AddKnownValue(keys[0]);
            GameManager.Instance.uiManager.SendUIToast("Temperature has been recorded. Fill it in the EHR field.", Color.white);
        }
    }
}