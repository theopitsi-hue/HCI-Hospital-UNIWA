using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FormUI : MonoBehaviour
{
    public UnityEngine.UI.Button submit;
    public GameObject fieldPrefab;
    public List<FormField> fields = new();
    public List<String> errorMessages = new();
    public TextMeshProUGUI errorLabel;
    public GameObject fieldContainer;
    DocumentationGate activeGate;
    public TextMeshProUGUI createReportTitle;

    private void Awake()
    {
        submit.onClick.AddListener(SubmitForm);
        GameManager.Instance.sceneExecutor.OnDocumentationGateSet.AddListener(OnDocuGateSet);
        GameManager.Instance.sceneExecutor.OnDocumentationGateCompleted.AddListener(OnDocuGateComplete);

        errorLabel.text = "";
    }

    private void OnDocuGateComplete(DocumentationGate gate)
    {
        activeGate = null;
        //clear all fields
        for (int i = fields.Count - 1; i >= 0; i--)
        {
            Destroy(fields[i].gameObject);
        }
        fields.Clear();
    }

    private void OnDocuGateSet(DocumentationGate gate)
    {
        if (activeGate != null)
            OnDocuGateComplete(activeGate);

        createReportTitle.text = "Report - Documentation Gate -" + gate.Label;
        Debug.Log("MADE FIELDS!!");
        activeGate = gate;
        //create all fields according to the gate


        for (int i = 0; i < gate.requiredFields.Count; i++)
        {
            var item = gate.requiredFields[i];
            CreateField(item, PrettifyName(item.name), 100, gate.awnsers[i].awnsers, gate.awnsers[i].correctAwnserId);
        }

        var possibleActions = GameManager.Instance.sceneExecutor.GetPossibleActionsToRecord();
        for (int i = 0; i < gate.requiredActionIds.Count; i++)
        {
            var id = gate.requiredActionIds[i];

            CreateField(null, "Action Taken", 100, possibleActions.Values.ToList(), possibleActions.Keys.ToList().IndexOf(id), true);
        }
    }

    public static string PrettifyName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        // Add spaces before capitals: "oxygenSaturation" -> "oxygen Saturation"
        name = Regex.Replace(name, @"(?<!^)([A-Z])", " $1");

        // Capitalize the first character
        return char.ToUpper(name[0]) + name.Substring(1);
    }

    private void SubmitForm()
    {
        //complete the active documentation gate
        if (NoErrorsFound() && activeGate != null)
        {
            //calc score and submit form, passing the gate.
            GameManager.Instance.sceneExecutor.AddScore(0, $"Documentation '{activeGate.Label}' Completed");
            //silly but it works
            foreach (var field in fields)
            {
                GameManager.Instance.sceneExecutor.AddScore(field.GetScore(),
                "\t" + field.label.text + " " +
                (field.HasCorrectSelected() ? "Filled Correctly" : "Filled Incorrectly")
                );
            }

            GameManager.Instance.sceneExecutor.ClearActiveDocumentationGate(activeGate, true);

            Debug.Log(GameManager.Instance.sceneExecutor.GetScoreReport());

            GameManager.Instance.playerData.Clear();
            GameManager.Instance.uiManager.GoToHUD();
        }

    }

    public void CreateField(BlackboardKey key, string label, int totalScore, List<string> possibleAwnsers, int rightAwnswerID, bool allowInvalid = false)
    {
        var nw = Instantiate(fieldPrefab, fieldContainer.transform);
        var formField = nw.GetComponent<FormField>();
        formField.Initialize(key, label, totalScore, possibleAwnsers, rightAwnswerID, allowInvalid);
        fields.Add(formField);
    }



    public bool ValidateFieldInformation()
    {
        bool allValid = true;

        errorMessages.Clear();
        foreach (var field in fields)
        {
            if (!field.HasValidInformation())
            {
                errorMessages.Add("Field " + field.label.text + " has not been filled!");
                allValid = false;
            }
        }

        return allValid;
    }

    public bool NoErrorsFound()
    {
        if (!ValidateFieldInformation())
        {
            //show error messages on text field
            StringBuilder sb = new();

            foreach (var error in errorMessages)
            {
                sb.Append(error);
                sb.Append("\n\n");
            }
            sb.Remove(sb.Length - 1, 1);

            errorLabel.text = sb.ToString();
            return false;
        }
        else
        {
            return true;
        }
    }


}
