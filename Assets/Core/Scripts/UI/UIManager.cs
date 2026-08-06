using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public enum UIType
    {
        None,
        MainMenu,
        PauseMenu,
        Settings,
        Help,
        Machines,
        HUD
    }

    [Serializable]
    public class UIEntry
    {
        public UIType type;
        public GameObject uiObject;

        [Header("Cursor")]
        public bool showCursor = true;
        public bool lockCursor = false;
    }

    [Header("Assign UI Objects")]
    [SerializeField] private List<UIEntry> uiEntries = new();

    private readonly Dictionary<UIType, UIEntry> uiDictionary = new();

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        BuildDictionary();
    }

    private void BuildDictionary()
    {
        uiDictionary.Clear();

        foreach (UIEntry entry in uiEntries)
        {
            if (entry.uiObject == null)
                continue;

            if (uiDictionary.ContainsKey(entry.type))
            {
                Debug.LogWarning($"Duplicate UI entry for {entry.type}");
                continue;
            }

            uiDictionary.Add(entry.type, entry);
        }
    }

    public void CloseAll()
    {
        foreach (UIEntry entry in uiDictionary.Values)
        {
            entry.uiObject.SetActive(false);
        }
    }

    public void Activate(UIType type)
    {
        if (uiDictionary.TryGetValue(type, out UIEntry entry))
        {
            entry.uiObject.SetActive(true);
            ApplyCursorSettings(entry);
        }
        else
        {
            Debug.LogWarning($"UI '{type}' not found.");
        }
    }

    public void ActivateOnly(UIType type)
    {
        CloseAll();
        Activate(type);
    }

    public void Deactivate(UIType type)
    {
        if (uiDictionary.TryGetValue(type, out UIEntry entry))
        {
            entry.uiObject.SetActive(false);
        }
    }

    public bool IsActive(UIType type)
    {
        return uiDictionary.TryGetValue(type, out UIEntry entry) &&
               entry.uiObject.activeSelf;
    }

    public GameObject GetUI(UIType type)
    {
        return uiDictionary.TryGetValue(type, out UIEntry entry)
            ? entry.uiObject
            : null;
    }

    private void ApplyCursorSettings(UIEntry entry)
    {
        Cursor.visible = entry.showCursor;
        Cursor.lockState = entry.lockCursor
            ? CursorLockMode.Locked
            : CursorLockMode.None;
    }
}