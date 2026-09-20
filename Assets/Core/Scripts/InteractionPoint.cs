using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(InteractReceiver))]
public class InteractionPoint : MonoBehaviour
{
    [SerializeField]
    public string interactionName;

    [SerializeField]
    public UIManager.UIType uiType;
    InteractReceiver rec;

    [SerializeField]
    public List<BlackboardKey> onClickObservations = new();

    [SerializeField]
    public RuleManager ruleManager = new();

    [SerializeReference, SubclassSelector]
    [Tooltip("Effects to trigger when clicking this.")]
    public List<Effect> justRun = new();
    private void Awake()
    {
        rec = GetComponent<InteractReceiver>();
        rec.onInteracted.AddListener(OnInteracted);
    }

    //curent bug: the ends for some fukin reason loop back to the first node and replay the whole scenario in an instant
    public virtual void OnInteracted()
    {
        if (!interactionName.Equals("_"))
        {
            if (!GameManager.Instance.sceneExecutor.CanUseHotSpot(interactionName))
            {
                GameManager.Instance.uiManager.SendUIToast("You cant use this right now.", Color.white);
                return;
            }
        }

        if (uiType != UIManager.UIType.None)
        {
            GameManager.Instance.uiManager.ActivateOnly(uiType);
        }

        ruleManager.EvaluateAll(GameManager.Instance.sceneExecutor);


        foreach (var item in justRun)
        {
            //Debug.Log("bro1");
            item.Apply(GameManager.Instance.sceneExecutor);
        }

        if (interactionName == "OxygenTank")
        {
            if (onClickObservations[0].TryGetValue(out var v))
            {
                float val = (float)v.GetValue();

                GameManager.Instance.uiManager.SendUIToast("Oxygen left: " + Mathf.RoundToInt(val) + "%", Color.white);
            }
        }
        else
        {
            foreach (var key in onClickObservations)
            {
                if (!GameManager.Instance.playerData.KnowsValue(key))
                {
                    GameManager.Instance.playerData.AddKnownValue(key);
                    GameManager.Instance.uiManager.SendUIToast($"{key.name} has been recorded. Fill it in the EHR field.", Color.white);
                }
            }

        }
    }
}
