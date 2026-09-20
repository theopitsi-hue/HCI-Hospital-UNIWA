using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ThermometerUI : MachineUI
{
    [SerializeField] private TMP_Text temperatureText;

    public override void Setup(InteractionPoint machine)
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
            GameManager.Instance.uiManager.SendUIToast(PrettifyName(keys[0].name)+" has been recorded. Fill it in the EHR field.", Color.white);
        }
    }

    public static string PrettifyName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        // Add spaces before capitals: "oxygenSaturation" -> "oxygen Saturation"
        name = Regex.Replace(name, @"(?<!^)([A-Z])", " $1");

        // Capitalize the first character
        return char.ToUpper(name[0]) + name.Substring(1);
    }
}