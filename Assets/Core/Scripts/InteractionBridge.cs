using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBridge : MonoBehaviour
{
    public string uiPanelName; //pedio (onoma panel)

    public void OpenUI()
    {
        MachineUIManagerController manager = FindFirstObjectByType<MachineUIManagerController>(); 
        
        if (manager != null)
        {
            manager.OpenPanelByName(uiPanelName);
        }
    }
}
