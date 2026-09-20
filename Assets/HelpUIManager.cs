using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HelpUIManager : MonoBehaviour
{
    public GameObject catButtonPrefab;
    public Transform catButtonHolder;
    private string activeCat;
    [SerializeField]
    private TMP_Text contentText;
    [SerializeField]
    private Color defaultColor, pressedColor;
    private bool initialized = false;

    Dictionary<string, Button> buttonIds = new();


    private void OnEnable()
    {
        SpawnAllBtns();

        if (activeCat == null)
            Activate(GameManager.Instance.sceneExecutor.TextMap.tutorials.Keys.ToList()[0]);
    }

    void SpawnAllBtns()
    {
        if (initialized) return;

        foreach (var item in GameManager.Instance.sceneExecutor.TextMap.tutorials)
        {
            SpawnButton(item.Key);
        }

        initialized = true;
    }

    void SpawnButton(string id)
    {
        if (buttonIds.ContainsKey(id)) return;

        var nb = Instantiate(catButtonPrefab, catButtonHolder);
        var bt = nb.GetComponent<Button>();
        buttonIds.Add(id, bt);
        bt.onClick.AddListener(() => { OnButtonPressed(id); });
        bt.GetComponentInChildren<TMP_Text>().text = id;
        SetBtnColor(id, defaultColor);
    }

    void OnButtonPressed(string id)
    {
        if (activeCat == null || !activeCat.Equals(id))
        {

            if (activeCat != null)
            {
                SetBtnColor(activeCat, defaultColor);
            }

            Activate(id);
            activeCat = id;
            SetBtnColor(id, pressedColor);
        }
    }

    void SetBtnColor(string id, Color clr)
    {
        var btn = buttonIds[id];
        var img = btn.gameObject.GetComponent<Image>();
        img.color = clr;
    }

    private void Activate(string id)
    {
        contentText.text = GameManager.Instance.sceneExecutor.TextMap.tutorials[id];
    }
}
