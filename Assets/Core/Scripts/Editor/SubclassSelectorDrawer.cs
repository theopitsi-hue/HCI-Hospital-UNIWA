#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    private static Dictionary<Type, Type[]> typeCache = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (!property.isArray && property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.LabelField(position, label.text, "Use [SerializeReference] with this attribute.");
            EditorGUI.EndProperty();
            return;
        }

        DrawManagedReference(position, property, label);

        EditorGUI.EndProperty();
    }

    private void DrawManagedReference(Rect position, SerializedProperty property, GUIContent label)
    {
        var baseType = GetFieldType();
        var types = GetConcreteTypes(baseType);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        Rect dropdownRect = new Rect(position.x, position.y, position.width, lineHeight);

        // Current type
        Type currentType = property.managedReferenceValue?.GetType();

        int currentIndex = Array.FindIndex(types, t => t == currentType);
        string[] typeNames = types.Select(t => t.Name).ToArray();

        int newIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex, typeNames);

        // Type changed
        if (newIndex != currentIndex)
        {
            property.managedReferenceValue = Activator.CreateInstance(types[newIndex]);
        }

        // Draw fields
        if (property.managedReferenceValue != null)
        {
            Rect fieldRect = new Rect(
                position.x,
                position.y + lineHeight + 2,
                position.width,
                EditorGUI.GetPropertyHeight(property, true)
            );

            if (property.managedReferenceValue is ChangeFlagEffect eff)
            {
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

            }
            else
            {
                EditorGUI.PropertyField(fieldRect, property, true);
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight;

        return EditorGUIUtility.singleLineHeight +
               2 +
               EditorGUI.GetPropertyHeight(property, true);
    }

    private Type GetFieldType()
    {
        var fieldType = fieldInfo.FieldType;

        if (fieldType.IsArray)
            return fieldType.GetElementType();

        if (fieldType.IsGenericType)
            return fieldType.GetGenericArguments()[0];

        return fieldType;
    }

    private Type[] GetConcreteTypes(Type baseType)
    {
        if (typeCache.TryGetValue(baseType, out var cached))
            return cached;

        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .ToArray();

        typeCache[baseType] = types;
        return types;
    }
}
#endif