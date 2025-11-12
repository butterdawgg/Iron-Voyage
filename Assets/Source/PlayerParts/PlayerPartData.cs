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

    public PlayerPart[] GetLockedParts()
    {
        List<PlayerPart> lockedParts = new();

        foreach (PlayerPart part in parts)
        {
            if (!SerializeManager.GetPartUnlocked(part))
                lockedParts.Add(part);
        }

        return lockedParts.ToArray();
    }

    public PlayerPart[] GetRandomAvailableParts(int count)
    {
        PlayerPart[] lockedParts = GetLockedParts();

        float roundId = SerializeManager.GetRound();

        List<PlayerPart> availableParts = new();

        foreach (PlayerPart part in lockedParts)
        {
            if (roundId >= part.availability.x && roundId <= part.availability.y)
                availableParts.Add(part);
        }

        if (availableParts.Count <= count)
            return availableParts.ToArray();

        int diff = availableParts.Count - count;
        System.Random rng = new ();

        while (diff > 0)
        {
            float totalInverseRarity = 0f;
            foreach (var part in availableParts)
                totalInverseRarity += 1f / Mathf.Max(part.rarity, 0.0001f); // avoid division by zero

            float pick = (float)rng.NextDouble() * totalInverseRarity;
            float cumulative = 0f;

            for (int i = 0; i < availableParts.Count; i++)
            {
                cumulative += 1f / Mathf.Max(availableParts[i].rarity, 0.0001f);
                if (pick <= cumulative)
                {
                    availableParts.RemoveAt(i);
                    diff--;
                    break;
                }
            }
        }

        return availableParts.ToArray();
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