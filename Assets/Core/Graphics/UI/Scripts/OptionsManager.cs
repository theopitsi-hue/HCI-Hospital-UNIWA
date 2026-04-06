using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    private bool isPaused = false;

    private void Start()
    {
        // arxizei klisto
        if (OptionsPanel != null && OptionsPanel.activeSelf)
        {
            OptionsPanel.SetActive(false);
            isPaused = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        OptionsPanel.SetActive(false);
        Time.timeScale = 1f; //συνεχιζει τον χρονο
        isPaused = false;

    }

    void Pause()
    {
        OptionsPanel.SetActive(true);
        Time.timeScale = 0f; //σταματαει τον χρονο
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
