using System.Collections.Generic;
using UnityEngine;

public class FormField : MonoBehaviour
{
    public int selectedAwnserID = 0;
    public BlackboardKey bbKey;
    public int rightAwnserID = -1;
    public int totalScoreReward = 0;
    public int wrongAwnserScoreDeduct = 0;
    public List<string> allPossibleAwnsers = new();

    public TMPro.TMP_Dropdown dropdown;
    public HighlightCorrect highlightCorrect;
    public TMPro.TextMeshProUGUI label;

    public void Initialize(BlackboardKey key, string label, int totalScore, List<string> possibleAwnsers, int rightAwnswerID)
    {
        this.label.text = label;
        bbKey = key;
        totalScoreReward = totalScore;
        wrongAwnserScoreDeduct = (totalScore / 3);
        this.rightAwnserID = rightAwnswerID + 1;

        highlightCorrect.redOptionIndex = this.rightAwnserID + 1;

        allPossibleAwnsers.Add("Not Selected.");
        allPossibleAwnsers.AddRange(possibleAwnsers);


        dropdown.ClearOptions();
        dropdown.AddOptions(allPossibleAwnsers);

        dropdown.onValueChanged.AddListener((i) => { selectedAwnserID = i; });

    }

    private void Update()
    {
        if (PlayerHasObservedAwnser())
        {
            highlightCorrect.HighlightAwnser();
        }
    }

    public bool PlayerHasObservedAwnser()
    {
        return GameManager.Instance.playerData.KnowsValue(bbKey);
    }

    public bool HasCorrectSelected()
    {
        return selectedAwnserID == rightAwnserID;
    }

    public int GetScore()
    {
        return HasCorrectSelected() ? rightAwnserID : wrongAwnserScoreDeduct;
    }

    public bool HasValidInformation()
    {
        return selectedAwnserID != 0;
    }
}