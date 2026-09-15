using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using System.IO;
[CustomEditor(typeof(ScenarioObject))]
public class ScenarioObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScenarioObject scenario = (ScenarioObject)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Export Scenario as JSON"))
        {
            string name = string.IsNullOrEmpty(scenario.scenarioMeta.id)
                ? "new_scenario"
                : scenario.scenarioMeta.id;

            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "HCIScenarios"
            );

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(
                folderPath,
                name + ".json"
            );

            File.WriteAllText(filePath, scenario.Serialize());

            Debug.Log($"Scenario exported to: {filePath}");

            EditorUtility.DisplayDialog(
                "Scenario Exported",
                $"Saved to:\n{filePath}",
                "OK"
            );
        }
    }
}