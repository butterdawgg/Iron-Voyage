using UnityEngine;

public abstract class PlayerPart : ScriptableObject
{
    [Tooltip("ID of the part type to resolve part selection conflicts")]
    public int typeId = 0;
    [Tooltip("ID of the part itself")]
    public int partId = 0;
    [Tooltip("Name of the part displayed in the shop")]
    public string displayName;
    [TextArea, Tooltip("Description of the part displayed in the shop")]
    public string description;

    public string GetStringId()
    {
        return "ship_part_type_" + typeId + "_id_" + partId;
    }

    // Override these for custom name and description processing:
    public virtual string GetDisplayName() { return displayName; }
    public virtual string GetDescription() { return description; }

    // Override these for the default parts:
    public virtual bool UnlockedByDefault() { return false; }
    public virtual bool SelectedByDefault() { return false; }

    public abstract void Apply(Player player);
}