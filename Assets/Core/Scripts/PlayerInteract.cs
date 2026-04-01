using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera pCam;
    public string interactTag = "Interactable";
    public int interactMaxDistance = 10;
    private InteractReceiver hovering;

    private void Update()
    {
        if (Physics.Raycast(pCam.transform.position, pCam.transform.forward, out var hit, interactMaxDistance))
        {
            if (hit.transform != null && hit.transform.TryGetComponent<InteractReceiver>(out var receiver))
            {
                if (hovering && receiver != hovering)
                {
                    hovering.TriggerHoverExit();
                }
                hovering = receiver;
                hovering.TriggerHoverEnter();
            }
            else
            {
                if (hovering)
                {
                    hovering.TriggerHoverExit();
                }
            }

        }
        else
        {
            if (hovering)
            {
                hovering.TriggerHoverExit();
            }
        }

        if (hovering != null && Input.GetMouseButtonDown(0))
        {
            hovering.TriggerInteraction();
        }
    }
}
