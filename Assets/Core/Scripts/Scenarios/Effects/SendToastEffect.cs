


using UnityEngine;

[System.Serializable]
public class SendToastEffect : Effect
{
    public string toastText = "";
    public Color color = Color.white;

    public override void Apply(ScenarioExecutor exec)
    {
        //base.Apply(exec);
        GameManager.Instance.uiManager.SendUIToast(toastText, color);
    }
}