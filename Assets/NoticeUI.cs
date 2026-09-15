using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button button;
    [Header("Button Callback")]
    [SerializeField] private UnityEvent onButtonPressed;
    private void Awake() { button.onClick.AddListener(OnButtonPressed); }

    private void OnButtonPressed()
    {
        onButtonPressed?.Invoke();
    }

    /// <summary>
    /// Allows a callback to be supplied from code. 
    /// </summary> 
    public void Setup(string title, string message, UnityAction callback)
    {
        titleText.text = title;
        messageText.text = message;
        // Remove previous runtime listeners 
        button.onClick.RemoveListener(OnButtonPressed);

        // Add the popup's internal handler 
        button.onClick.AddListener(OnButtonPressed);
        // Add the user's callback 
        if (callback != null) { button.onClick.AddListener(callback); }
        gameObject.SetActive(true);
    }
}
