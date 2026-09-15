


using UnityEngine;

[System.Serializable]
public class NotifyUIEffect : Effect
{
    public string toastText = "";


    public override void Apply(ScenarioExecutor exec)
    {
        //base.Apply(exec);
        GameManager.Instance.uiManager.SendUIToast(toastText, UnityEngine.Color.white);
        Debug.Log("SHOULD BE: " + toastText);
    }
}