using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(InteractReceiver))]
public class InteractionPoint : MonoBehaviour
{
    [SerializeField]
    public UIManager.UIType uiType;
    InteractReceiver rec;

    [SerializeField]
    public List<BlackboardKey> onClickObservations = new();

    private void Awake()
    {
        rec = GetComponent<InteractReceiver>();
        rec.onInteracted.AddListener(OnInteracted);
    }


    public virtual void OnInteracted()
    {
        if (uiType != UIManager.UIType.None)
        {
            GameManager.Instance.uiManager.ActivateOnly(uiType);
        }

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
