using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    private RectTransform contentRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
    }

    private void LateUpdate()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null)
            return;

        var selectedRect = selected.GetComponent<RectTransform>();
        if (selectedRect == null || !selectedRect.IsChildOf(contentRect))
            return;

        ScrollToKeepInView(selectedRect);
    }

    private void ScrollToKeepInView(RectTransform target)
    {
        RectTransform viewport = scrollRect.viewport;

        // Convert to world space
        Vector3[] viewportCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);
        target.GetWorldCorners(targetCorners);

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;
        float targetTop = targetCorners[1].y;
        float targetBottom = targetCorners[0].y;

        float contentHeight = contentRect.rect.height;
        float viewportHeight = viewport.rect.height;

        float delta = 0f;

        // Scroll up
        if (targetTop > viewportTop)
            delta = targetTop - viewportTop;

        // Scroll down
        else if (targetBottom < viewportBottom)
            delta = targetBottom - viewportBottom;

        if (Mathf.Approximately(delta, 0f))
            return;

        // Adjust verticalNormalizedPosition instantly
        float normalizedDelta = delta / (contentHeight - viewportHeight);
        float newPos = scrollRect.verticalNormalizedPosition + normalizedDelta;

        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPos);
    }
}
