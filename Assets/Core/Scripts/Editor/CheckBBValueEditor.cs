using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckBBValue))]
public class CheckBBValueEditor : Editor
{
    SerializedProperty comparisonMode;
    SerializedProperty left;
    SerializedProperty right;
    SerializedProperty rightNum;
    SerializedProperty rightBool;
    SerializedProperty numberComparisonType;
    SerializedProperty booleanComparisonType;

    void OnEnable()
    {
        comparisonMode = serializedObject.FindProperty("comparisonMode");
        left = serializedObject.FindProperty("left");
        right = serializedObject.FindProperty("right");
        rightNum = serializedObject.FindProperty("rightNum");
        rightBool = serializedObject.FindProperty("rightBool");
        numberComparisonType = serializedObject.FindProperty("numberComparisonType");
        booleanComparisonType = serializedObject.FindProperty("booleanComparisonType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Optional mode selector (keep or remove if you want fully implicit UI)
        EditorGUILayout.PropertyField(comparisonMode);

        EditorGUILayout.Space(4);

        var mode = (ComparisonMode)comparisonMode.enumValueIndex;

        EditorGUILayout.BeginHorizontal();

        // LEFT SIDE (always BlackboardKey)
        EditorGUILayout.PropertyField(left, GUIContent.none);

        // COMPARATOR (depends on mode)
        if (mode == ComparisonMode.NUMBER || mode == ComparisonMode.FLAT_NUMBER)
        {
            EditorGUILayout.PropertyField(numberComparisonType, GUIContent.none, GUILayout.Width(100));
        }
        else
        {
            EditorGUILayout.PropertyField(booleanComparisonType, GUIContent.none, GUILayout.Width(100));
        }

        // RIGHT SIDE (dynamic input)
        switch (mode)
        {
            case ComparisonMode.NUMBER:
            case ComparisonMode.BOOL:
                EditorGUILayout.PropertyField(right, GUIContent.none);
                break;

            case ComparisonMode.FLAT_NUMBER:
                EditorGUILayout.PropertyField(rightNum, GUIContent.none);
                break;

            case ComparisonMode.FLAT_BOOL:
                EditorGUILayout.PropertyField(rightBool, GUIContent.none);
                break;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4);

        serializedObject.ApplyModifiedProperties();
    }


}