using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUIManagerController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject hrPanel;
    public GameObject o2Panel;
    public GameObject bpPanel;
    public GameObject tempPanel;

    private List<GameObject> allPanels;

    private void Awake()
    {
        allPanels = new List<GameObject> { hrPanel, o2Panel, bpPanel, tempPanel };
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        //xronos jana
        Time.timeScale = 1f;
    }

    public void OpenSpecificPanel(GameObject panelToOpen)
    {
        CloseAllPanels(); //Ta kleinw ola 

        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true); //Anoigw auto pou prepei
            Time.timeScale = 0f;//kanw freeze to game alla mporei na mh xreiastei
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OpenPanelByName(string name)
    {
        if (name == "HR") OpenSpecificPanel(hrPanel);
        else if (name == "O2") OpenSpecificPanel(o2Panel);
        else if (name == "BP") OpenSpecificPanel(bpPanel);
        else if (name == "Temp") OpenSpecificPanel(tempPanel);
        else Debug.LogWarning("Το όνομα " + name + " δεν βρέθηκε!");
    }

    public void OpenHrMachine() => OpenSpecificPanel(hrPanel);
    public void OpenO2Machine() => OpenSpecificPanel(o2Panel);
    public void OpenBpMachine() => OpenSpecificPanel(bpPanel);
    public void OpenTemp() => OpenSpecificPanel(tempPanel);
}