#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CheckFlagCondition), true)]
public class CheckBBValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float line = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        // Draw label
        Rect labelRect = new Rect(position.x, y, position.width, line);
        EditorGUI.LabelField(labelRect, label);
        y += line + 2;

        SerializedProperty comparisonMode = property.FindPropertyRelative("comparisonMode");
        SerializedProperty left = property.FindPropertyRelative("left");
        SerializedProperty right = property.FindPropertyRelative("right");
        SerializedProperty rightNum = property.FindPropertyRelative("rightNum");
        SerializedProperty rightBool = property.FindPropertyRelative("rightBool");
        SerializedProperty numberComparisonType = property.FindPropertyRelative("numberComparisonType");
        SerializedProperty booleanComparisonType = property.FindPropertyRelative("booleanComparisonType");

        if (comparisonMode == null || left == null)
        {
            EditorGUI.LabelField(new Rect(position.x, y, position.width, line),
                "Missing serialized fields");
            EditorGUI.EndProperty();
            return;
        }

        var mode = (ComparisonMode)comparisonMode.enumValueIndex;

        var r = new Rect(position.x, position.y + 12, position.width / 5, position.height);
        EditorGUI.BeginProperty(r, GUIContent.none, left);
        EditorGUI.PropertyField(r, comparisonMode, GUIContent.none);
        EditorGUI.EndProperty();
        y += EditorGUIUtility.singleLineHeight + 4 + 12 + 5;

        // LEFT
        Rect row = new Rect(position.x, y, position.width / 3f, line);
        EditorGUI.BeginProperty(row, GUIContent.none, left);
        EditorGUI.PropertyField(row, left, GUIContent.none);
        EditorGUI.EndProperty();

        BlackboardValueType leftType = BlackboardValueType.BOOL;

        if (left == null || left.objectReferenceValue == null)
            leftType = BlackboardValueType.BOOL;

        if (left.objectReferenceValue is BlackboardKey key)
            leftType = key.type;

        bool isNumber = leftType == BlackboardValueType.FLOAT;
        bool isBool = leftType == BlackboardValueType.BOOL;

        Rect compRect = new Rect(position.x + position.width / 3f, y, position.width / 3f, line);
        Rect rRow = new Rect(position.x + (position.width / 3f) * 2f, y, position.width / 3f, line);
        // Comparator
        if (isNumber)
        {
            EditorGUI.PropertyField(compRect, numberComparisonType, GUIContent.none);
        }
        else if (isBool)
        {
            EditorGUI.PropertyField(compRect, booleanComparisonType, GUIContent.none);
        }

        switch (mode)
        {
            case ComparisonMode.VARIABLE:
                EditorGUI.PropertyField(rRow, right, GUIContent.none);
                break;

            case ComparisonMode.FLAT:
                if (isNumber)
                    EditorGUI.PropertyField(rRow, rightNum, GUIContent.none);
                else if (isBool)
                    EditorGUI.PropertyField(rRow, rightBool, GUIContent.none);
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4 + 8;
    }
}
#endif