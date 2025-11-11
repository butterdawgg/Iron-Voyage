using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewShipPart", menuName = "Player/PartData")]
public class PlayerPartData : ScriptableObject
{
    [SerializeField] private PlayerPart[] parts;

    public PlayerPart[] GetSelectedParts()
    {
        List<PlayerPart> selectedParts = new ();

        foreach (PlayerPart part in parts)
        {
            if (SerializeManager.GetPartSelected(part))
                selectedParts.Add(part);
        }

        return selectedParts.ToArray();
    }

    public PlayerPart[] GetUnlockedParts()
    {
        List<PlayerPart> unlockedParts = new();

        foreach (PlayerPart part in parts)
        {
            if (SerializeManager.GetPartUnlocked(part))
                unlockedParts.Add(part);
        }

        return unlockedParts.ToArray();
    }

    public void SelectPart(PlayerPart part)
    {
        SerializeManager.SetPartSelected(part, true);

        foreach (PlayerPart p in parts)
        {
            if (p.typeId == part.typeId && p.partId != part.partId)
                SerializeManager.SetPartSelected(p, false);
        }
    }

    public void UnlockPart(PlayerPart part)
    {
        SerializeManager.SetPartUnlocked(part, true);
    }
}