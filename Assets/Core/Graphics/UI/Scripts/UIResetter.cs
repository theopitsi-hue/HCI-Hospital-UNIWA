using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIResetter : MonoBehaviour
{
    private Button[] allButtons;

    void Awake()
    {
        // Παίρνει όλα τα κουμπιά που βρίσκονται μέσα στο Panel
        allButtons = GetComponentsInChildren<Button>(true);
    }

    void OnEnable()
    {
        if (allButtons == null) return;

        foreach (var btn in allButtons)
        {
            // 1. Καθαρίζουμε το visual state χειροκίνητα
            btn.targetGraphic.CrossFadeColor(btn.colors.normalColor, 0f, true, true);
            
            // 2. Αν χρησιμοποιείς Animator, του λέμε να πάει στο Normal state
            if (btn.animator != null)
            {
                btn.animator.Play("Normal", 0, 0f);
            }
        }

        // 3. Ξεχνάμε την επιλογή από το EventSystem
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}