using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public abstract class Action
{
    [SerializeField] private float delay = 0f;

    public float Delay { get { return delay; } }
    public virtual void Init() { }
    public abstract void Perform();
}

[System.Serializable]
public class LoadScene : Action
{
    [SerializeField] private int sceneId;

    public override void Perform()
    {
        SceneManager.LoadScene(sceneId);
    }
}

[System.Serializable]
public class SetObjectActive : Action
{
    [SerializeField] private GameObject obj;
    [SerializeField] private bool active;

    public override void Perform()
    {
        obj.SetActive(active);
    }
}

[System.Serializable]
public class ResetProgress : Action
{
    public override void Perform()
    {
        SerializeManager.ResetProgress();
    }
}

[System.Serializable]
public class QuitGame : Action
{
    public override void Perform()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class SetButtonInteractable : Action
{
    [SerializeField] private Button button;
    [SerializeField] private bool interactable;

    public override void Perform()
    {
        button.interactable = interactable;
    }
}

[System.Serializable]
public class SetFirstPlay : Action
{
    public override void Perform()
    {
        SerializeManager.SetFirstPlay(false);
    }
}