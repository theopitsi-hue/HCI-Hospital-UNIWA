#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ChangeFlagEffect), true)]
public class SetBBValueEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel++;

        Rect fieldRect = new Rect(
               position.x,
               position.y + EditorGUIUtility.singleLineHeight + 2,
               position.width,
               EditorGUI.GetPropertyHeight(property, true)
           );


        var flagProp = property.FindPropertyRelative("flag");
        var floatProp = property.FindPropertyRelative("floatValue");
        var boolProp = property.FindPropertyRelative("boolValue");

        bool showValueFields = flagProp != null;
        bool showFloatValue = false;
        bool showBoolValue = false;
        BlackboardValueType key = BlackboardValueType.BOOL;

        if (flagProp.objectReferenceValue != null)
        {
            key = (flagProp.objectReferenceValue as BlackboardKey).type;
            showFloatValue = key == BlackboardValueType.FLOAT;
            showBoolValue = key == BlackboardValueType.BOOL;
        }

        var iterator = property.Copy();
        var end = property.GetEndProperty();

        iterator.NextVisible(true);

        float y = fieldRect.y;

        while (!SerializedProperty.EqualContents(iterator, end))
        {
            float h = EditorGUI.GetPropertyHeight(iterator, true);

            Rect r = new Rect(fieldRect.x, y, fieldRect.width, h);


            if (iterator.name == "boolValue" && showBoolValue)
            {
                EditorGUI.PropertyField(r, iterator, true);
                y += h + 2;
            }
            if (iterator.name == "floatValue" && showFloatValue)
            {
                EditorGUI.PropertyField(r, iterator, true);
                y += h + 2;
            }

            if (!(iterator.name == "boolValue" || iterator.name == "floatValue"))
            {
                EditorGUI.PropertyField(r, iterator, true);

                y += h + 2;
            }

            iterator.NextVisible(false);
        }
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight * 6) + 4;
    }

}
#endif