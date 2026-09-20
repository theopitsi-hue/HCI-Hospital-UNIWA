using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighlightCorrect : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] public int redOptionIndex = 2;

    [SerializeField] Color color = Color.green;

    public void HighlightAwnser()
    {
        if (!dropdown.IsExpanded)
            return;

        Transform list = dropdown.transform.Find("Dropdown List");

        if (list == null)
            return;

        TMP_Text[] optionTexts = list.GetComponentsInChildren<TMP_Text>(true);

        if (redOptionIndex >= 0 && redOptionIndex < optionTexts.Length)
        {
            optionTexts[redOptionIndex].color = color;
            optionTexts[redOptionIndex].fontStyle = FontStyles.Bold;
        }
    }
}
