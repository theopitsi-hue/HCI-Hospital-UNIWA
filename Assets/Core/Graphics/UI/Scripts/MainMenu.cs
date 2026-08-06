using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject buttons;
    public GameObject buttonContainer;

    public GameObject button_prefab;

    public void OnEnable()
    {
        //spawn all the level buttons
        GotoButtons();
        GameManager.Instance.loader.onScenariosLoaded.AddListener(OnScenariosLoaded);
        OnScenariosLoaded();
    }

    private void OnScenariosLoaded()
    {
        //despawn all previous buttons
        for (int i = buttonContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonContainer.transform.GetChild(i).gameObject);
        }

        //spawn all the level buttons
        foreach (var item in GameManager.Instance.loader.loadedScenarios)
        {
            GameObject button = Instantiate(button_prefab, buttonContainer.transform);
            LevelSelectCompUI levelSelectCompUI = button.GetComponent<LevelSelectCompUI>();
            levelSelectCompUI.Initialize(item);
        }

    }

    public void ReloadScenarios()
    {
        GameManager.Instance.loader.Reload();
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

  
}
