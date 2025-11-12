using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerPartData partData;
    [SerializeField] private PartUnlockWindow partUnlockWindowPrefab;
    [SerializeField] private PartSelectWindow partSelectWindowPrefab;

    [SerializeField] private GameObject partUnlockPanel;
    [SerializeField] private GameObject partSelectPanel;

    [SerializeField] private GameObject partUnlockParent;
    [SerializeField] private GameObject partSelectParent;

    [SerializeField] private Button partUnlockButton;
    [SerializeField] private Button partSelectButton;

    private List<PartUnlockWindow> partUnlockWindows = new();
    private List<PartSelectWindow> partSelectWindows = new();

    // Caching parts required due to random selection
    private List<PlayerPart> availableParts;

    private void Awake()
    {
        availableParts = partData.GetRandomAvailableParts(5).ToList();

        partUnlockButton.onClick.AddListener(OnPartUnlockButtonClick);
        partSelectButton.onClick.AddListener(OnPartSelectButtonClick);

        OnPartUnlockButtonClick();
    }

    private void Update()
    {
        foreach (var window in partUnlockWindows)
        {
            window.UpdateWindow();
        }

        foreach (var window in partSelectWindows)
        {
            window.UpdateWindow();
        }
    }

    private void CreatePartUnlockWindows()
    {
        availableParts.RemoveAll(part => SerializeManager.GetPartUnlocked(part));

        foreach (var window in partUnlockWindows)
        {
            Destroy(window.gameObject);
        }

        partUnlockWindows.Clear();

        foreach (var part in availableParts)
        {
            var window = Instantiate(partUnlockWindowPrefab,
                partUnlockParent.transform);

            window.Initialize(partData, part);

            partUnlockWindows.Add(window);
        }
    }

    private void CreatePartSelectWindows()
    {
        foreach (var window in partSelectWindows)
        {
            Destroy(window.gameObject);
        }

        partSelectWindows.Clear();

        PlayerPart[] unlockedParts = partData.GetUnlockedParts();

        foreach (var part in unlockedParts)
        {
            var window = Instantiate(partSelectWindowPrefab,
                partSelectParent.transform);

            window.Initialize(partData, part);

            partSelectWindows.Add(window);
        }
    }

    private void OnPartUnlockButtonClick()
    {
        partUnlockButton.interactable = false;
        partSelectButton.interactable = true;

        partUnlockPanel.SetActive(true);
        partSelectPanel.SetActive(false);

        CreatePartUnlockWindows();
    }

    private void OnPartSelectButtonClick()
    {
        partUnlockButton.interactable = true;
        partSelectButton.interactable = false;

        partUnlockPanel.SetActive(false);
        partSelectPanel.SetActive(true);

        CreatePartSelectWindows();
    }
}