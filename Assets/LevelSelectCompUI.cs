using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectCompUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;
    public ScenarioObject scenarioObject;

    private string levelName;


    public void Initialize(ScenarioObject scenario)
    {
        this.text.text = scenario.scenarioMeta.id + "\nDifficulty: " + scenario.scenarioMeta.difficulty;
        this.levelName = scenario.scenarioMeta.levelName;
        button.onClick.AddListener(listenerCreator);
    }

    public void listenerCreator() { GameManager.Instance.StartLevel(levelName); }
}
