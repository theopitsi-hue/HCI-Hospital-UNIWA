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
        InteractReceiver receiver = null;

        // Raycast from the center of the camera
        if (Physics.Raycast(
            pCam.transform.position,
            pCam.transform.forward,
            out RaycastHit hit,
            interactMaxDistance))
        {
            // Check the object we hit
            if (hit.transform.TryGetComponent<InteractReceiver>(out var hitReceiver))
            {
                receiver = hitReceiver;
            }
        }

        // We are now hovering a different object
        if (receiver != hovering)
        {
            // Stop hovering the old object
            if (hovering != null)
            {
                hovering.TriggerHoverExit();
            }

            // Start hovering the new object
            hovering = receiver;

            if (hovering != null)
            {
                hovering.TriggerHoverEnter();
            }
        }

        // Interact only with the object we're CURRENTLY hovering
        if (hovering != null &&
            Input.GetMouseButtonDown(0) &&
            Cursor.visible == false)
        {
            hovering.TriggerInteraction();
        }
    }
}
