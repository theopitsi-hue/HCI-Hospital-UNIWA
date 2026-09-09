


using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SendNoticeEffect : Effect
{
    public string noticeTitle = "";
    public string noticeText = "";

    [SerializeReference, SubclassSelector]
    [Tooltip("Effects to trigger when pressing the button.")]
    public List<Effect> onButtonPressedEffects = new();

    public override void Apply(ScenarioExecutor exec)
    {
        //base.Apply(exec);
        GameManager.Instance.uiManager.SendUINotice(noticeTitle, noticeText, () =>
        {
            foreach (var item in onButtonPressedEffects)
            {
                item.Apply(exec);
            }
        });
    }
}