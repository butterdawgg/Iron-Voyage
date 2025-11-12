using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEnsureSelection : MonoBehaviour
{
    [SerializeField] private Selectable[] defaultSelections;

    private void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        if (current == null ||
            !current.TryGetComponent(out Selectable sel) ||
            !sel.IsInteractable())
        {
            foreach (var s in defaultSelections)
            {
                if (s.IsInteractable() && s.gameObject.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(s.gameObject);

                    break;
                }
            }
        }
    }
}