using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitButton(){
        Application.Quit();
        Debug.Log("Game closed.");
    }

    public void Level0(){
        SceneManager.LoadScene("level0");
        SceneManager.LoadScene("_globalScripts", LoadSceneMode.Additive);
    }
}
