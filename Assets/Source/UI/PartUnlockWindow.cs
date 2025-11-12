using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartUnlockWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float nameTextRelativeSize;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float descriptionTextRelativeSize;
    [SerializeField] private TextMeshProUGUI unlockButtonText;
    [SerializeField] private float unlockButtonTextRelativeSize;

    [SerializeField] private Button unlockButton;

    private RectTransform rect;

    private PlayerPartData partData;
    private PlayerPart part;

    private void OnUnlockButtonClick()
    {
        int money = SerializeManager.GetMoney();

        if (money < part.unlockPrice)
            return;

        partData.UnlockPart(part);

        SerializeManager.SetMoney(money - part.unlockPrice);
    }

    public void Initialize(PlayerPartData partData, PlayerPart part)
    {
        this.partData = partData;
        this.part = part;

        unlockButton.onClick.AddListener(OnUnlockButtonClick);

        rect = GetComponent<RectTransform>();

        nameText.enableAutoSizing = true;
        nameText.text = part.displayName;
        nameText.fontSizeMax = rect.sizeDelta.x
            * nameTextRelativeSize;

        descriptionText.enableAutoSizing = true;
        descriptionText.text = part.description;
        descriptionText.fontSizeMax = rect.sizeDelta.x *
            descriptionTextRelativeSize;

        unlockButtonText.enableAutoSizing = true;
        unlockButtonText.fontSizeMax = rect.sizeDelta.x *
            unlockButtonTextRelativeSize;

        UpdateWindow();
    }

    public void UpdateWindow()
    {
        if (SerializeManager.GetPartUnlocked(part))
        {
            unlockButton.interactable = false;
            unlockButtonText.text = "UNLOCKED";
        }
        else
        {
            unlockButton.interactable = true;
            unlockButtonText.text = "UNLOCK $" + part.unlockPrice;
        }
    }
}