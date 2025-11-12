using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Condition
{
    protected bool IsSatisfied { get; set; }
    public virtual void Init() { }
    public abstract bool Check();
}

[System.Serializable]
public class ButtonClick : Condition
{
    [SerializeField] private Button button;
    [SerializeField] private bool resets;

    public override void Init()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    ~ButtonClick()
    {
        if (button != null)
            button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        IsSatisfied = true;
    }

    public override bool Check()
    {
        bool result = IsSatisfied;

        if (resets && result)
            IsSatisfied = false;

        return result;
    }
}

[System.Serializable]
public class OnFirstPlay : Condition
{
    private bool hasActivated;

    public override bool Check()
    {
        if (SerializeManager.GetFirstPlay() && !hasActivated)
        {
            hasActivated = true;
            return true;
        }

        return false;
    }
}