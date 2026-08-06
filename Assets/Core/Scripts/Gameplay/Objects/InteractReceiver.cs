using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractReceiver : MonoBehaviour
{
    private bool _interactable = true;

    public UnityEvent onInteracted;
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;

    private void Awake()
    {
        if (_interactable)
        {
            tag = "Interactable";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void TriggerInteraction()
    {
        onInteracted?.Invoke();
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

    public bool SetInteractable(bool interactable)
    {
        _interactable = interactable;
        if (_interactable)
        {
            tag = "Interactable";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        return _interactable;
    }

}
