using System.Collections;
using UnityEngine;

public class CustomEvent : MonoBehaviour
{
    [SerializeField] private bool isRepeatable;
    [SerializeReference, SubclassSelector]
    private Condition[] conditions;
    [SerializeReference, SubclassSelector]
    private Action[] actions;

    private bool performed;
    private bool isPerforming;

    private void Awake()
    {
        foreach (var condition in conditions)
            condition.Init();

        foreach (var action in actions)
            action.Init();
    }

    private void Update()
    {
        if (!isRepeatable && performed)
            return;

        if (isPerforming)
            return;

        if (CheckConditions())
            PerformActions();
    }

    private bool CheckConditions()
    {
        foreach (var condition in conditions)
        {
            if (!condition.Check())
                return false;
        }

        return true;
    }

    private void PerformActions()
    {
        performed = true;

        StartCoroutine(PerformActionsCoroutine());
    }

    private IEnumerator PerformActionsCoroutine()
    {
        isPerforming = true;

        foreach (var action in actions)
        {
            if (action.Delay > 0f)
                yield return new WaitForSeconds(action.Delay);

            action.Perform();
        }

        isPerforming = false;
    }
}