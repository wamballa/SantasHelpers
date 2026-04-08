using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private bool updateContinuously;

    private RectTransform rectTransform;
    private Rect lastAppliedSafeArea = Rect.zero;
    private Vector2Int lastScreenSize = Vector2Int.zero;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeAreaIfNeeded(true);
    }

    private void OnEnable()
    {
        ApplySafeAreaIfNeeded(true);
    }

    private void Update()
    {
        if (updateContinuously)
        {
            ApplySafeAreaIfNeeded(false);
        }
    }

    private void ApplySafeAreaIfNeeded(bool force)
    {
        Rect safeArea = Screen.safeArea;
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

        if (!force && safeArea == lastAppliedSafeArea && screenSize == lastScreenSize)
        {
            return;
        }

        lastAppliedSafeArea = safeArea;
        lastScreenSize = screenSize;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
