using System;
using UnityEngine;

public static class SerializeManager
{
    private static bool GetBool(string key, bool defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            PlayerPrefs.SetInt(key, Convert.ToInt32(defaultValue));

        return Convert.ToBoolean(PlayerPrefs.GetInt(key));
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    private static int GetInt(string key, int defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            PlayerPrefs.SetInt(key, defaultValue);

        return PlayerPrefs.GetInt(key);
    }

    private static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public static void SetPartSelected(PlayerPart part, bool selected)
    {
        string key = part.GetStringId() + "selected";

        SetBool(key, selected);
    }

    public static bool GetPartSelected(PlayerPart part)
    {
        string key = part.GetStringId() + "selected";

        return GetBool(key, part.SelectedByDefault());
    }

    public static void SetPartUnlocked(PlayerPart part, bool unlocked)
    {
        string key = part.GetStringId() + "unlocked";

        SetBool(key, unlocked);
    }

    public static bool GetPartUnlocked(PlayerPart part)
    {
        string key = part.GetStringId() + "unlocked";

        return GetBool(key, part.SelectedByDefault());
    }

    public static void SetRound(int roundNumber)
    {
        SetInt("round_number", roundNumber);
    }

    public static int GetRound()
    {
        return GetInt("round_number", 0);
    }

    public static bool GetFirstPlay()
    {
        return GetBool("first_play", true);
    }

    public static void SetFirstPlay(bool isFirstPlay)
    {
        SetBool("first_play", isFirstPlay);
    }
}