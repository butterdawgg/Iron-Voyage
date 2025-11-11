using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    // The name of the array field that will be checked
    public string ConditionalArrayField = "";
    // TRUE = Hide in inspector / FALSE = Disable in inspector
    public bool HideInInspector = false;
    // TRUE = invert logic (empty = editable, not empty = not editable)
    public bool Inverse = false;

    public ConditionalHideAttribute(string conditionalArrayField)
    {
        this.ConditionalArrayField = conditionalArrayField;
        this.HideInInspector = false;
        this.Inverse = false;
    }

    public ConditionalHideAttribute(string conditionalArrayField, bool hideInInspector, bool inverse = false)
    {
        this.ConditionalArrayField = conditionalArrayField;
        this.HideInInspector = hideInInspector;
        this.Inverse = inverse;
    }
}