using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScroreUIManager : MonoBehaviour
{
    // public Button BackToMenu;
    public Button QuitGame;

    public TMP_Text scoreText;
    public TMP_Text decisionTreeText;
    public TMP_Text scoreTotal;

    private void Awake()
    {
        //    BackToMenu.onClick.AddListener(BackToMenuF);
        QuitGame.onClick.AddListener(QuitGameF);
    }

    private void OnEnable()
    {

        DisplayResults();
    }

    public void DisplayResults()
    {
        var exec = GameManager.Instance.sceneExecutor;
        scoreText.text = exec.GetScoreReport();
        decisionTreeText.text = exec.GetDecisionReport();

        int mScore = exec.GetTotalPossibleScore();
        int cScore = exec.GetTotalScore();

        scoreTotal.text = $"Total: {cScore}/{mScore}";
    }

    private void QuitGameF()
    {
        Application.Quit();
    }

    private void BackToMenuF()
    {
        GameManager.Instance.QuitLevel();
    }
}
