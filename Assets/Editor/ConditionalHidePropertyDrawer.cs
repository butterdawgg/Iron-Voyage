using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!condHAtt.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalArrayField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    enabled = sourcePropertyValue.boolValue;
                    break;

                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Generic:
                    if (sourcePropertyValue.isArray)
                    {
                        enabled = sourcePropertyValue.arraySize > 0;
                    }
                    break;

                default:
                    Debug.LogWarning($"ConditionalHideAttribute: '{condHAtt.ConditionalArrayField}' is not a boolean or array, defaulting to enabled.");
                    break;
            }

            // Apply inverse logic
            if (condHAtt.Inverse)
            {
                enabled = !enabled;
            }
        }
        else
        {
            Debug.LogWarning("ConditionalHideAttribute: No matching SourcePropertyValue found in object: " + condHAtt.ConditionalArrayField);
        }

        return enabled;
    }

}
