using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TextJason : MonoBehaviour
{
    public string fileName = "scenario.json";
    public TextMeshProUGUI dialouge;
    int Current_text=0;

    void Start()
    {
        LoadText();
    }

   void LoadText()
    {
     
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            ScenarioWrapper scenarioData = JsonUtility.FromJson<ScenarioWrapper>(json);
            if (scenarioData.intro.messages.Length > Current_text)
                dialouge.text = scenarioData.intro.messages[Current_text];
            else
                dialouge.text="end";
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }


    public void Next_Text()
    {
        Current_text++;
        LoadText();
    }
}


