using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractReceiver : MonoBehaviour
{
    public UnityEvent onInteracted;
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;

    private void Awake()
    {
        tag = "Interactable";
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void TriggerInteraction()
    {
        onInteracted?.Invoke();
        Debug.Log("BAM");
    }

    public void TriggerHoverExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        OnHoverExit?.Invoke();
    }

    public void TriggerHoverEnter()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline1");
        OnHoverEnter?.Invoke();
    }
}
