using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    public GameObject HelpPanelObj;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        HelpPanelObj.SetActive(false);
        Time.timeScale = 1f; //συνεχιζει τον χρονο
        isPaused = false;
        
    }

    void Pause()
    {
        HelpPanelObj.SetActive(true); 
        Time.timeScale = 0f; //σταματαει τον χρονο
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
