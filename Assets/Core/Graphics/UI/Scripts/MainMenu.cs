using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject buttons;

    public void OnEnable()
    {
        GotoButtons();
    }

    public void GotoLevelSelect()
    {
        levelSelect.SetActive(true);
        buttons.SetActive(false);
    }

    public void GotoButtons()
    {
        levelSelect.SetActive(false);
        buttons.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void PlayLevel(string name)
    {
        GameManager.Instance.StartLevel(name);
    }
}
