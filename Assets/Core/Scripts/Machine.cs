using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(InteractReceiver))]
public class Machine : MonoBehaviour
{
    [SerializeField]
    public UIManager.UIType uiType;
    InteractReceiver rec;

    private void Awake()
    {
        rec = GetComponent<InteractReceiver>();
        rec.onInteracted.AddListener(OnInteracted);
    }


    public void OnInteracted()
    {
        GameManager.Instance.uiManager.ActivateOnly(uiType);
    }
}
