using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartSelectWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float nameTextRelativeSize;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float descriptionTextRelativeSize;
    [SerializeField] private TextMeshProUGUI selectButtonText;
    [SerializeField] private float selectButtonTextRelativeSize;

    [SerializeField] private Button selectButton;

    private RectTransform rect;

    private PlayerPartData partData;
    private PlayerPart part;

    private void OnSelectButtonClick()
    {
        partData.SelectPart(part);

        selectButton.interactable = false;
        selectButtonText.text = "SELECTED";
    }

    public void Initialize(PlayerPartData partData, PlayerPart part)
    {
        this.partData = partData;
        this.part = part;

        selectButton.onClick.AddListener(OnSelectButtonClick);

        rect = GetComponent<RectTransform>();

        nameText.enableAutoSizing = true;
        nameText.text = part.displayName;
        nameText.fontSizeMax = rect.sizeDelta.x
            * nameTextRelativeSize;

        descriptionText.enableAutoSizing = true;
        descriptionText.text = part.description;
        descriptionText.fontSizeMax = rect.sizeDelta.x *
            descriptionTextRelativeSize;

        selectButtonText.enableAutoSizing = true;
        selectButtonText.fontSizeMax = rect.sizeDelta.x *
            selectButtonTextRelativeSize;

        UpdateWindow();
    }

    public void UpdateWindow()
    {
        if (SerializeManager.GetPartSelected(part))
        {
            selectButton.interactable = false;
            selectButtonText.text = "SELECTED";
        }
        else
        {
            selectButton.interactable = true;
            selectButtonText.text = "SELECT";
        }
    }
}