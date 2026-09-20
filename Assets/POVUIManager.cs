using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages a dictionary of points of interest on screen with UI indicators.
/// Points are shown on screen if visible, or anchored to screen edges if off-screen.
/// </summary>
public class POVUIManager : MonoBehaviour
{
    [System.Serializable]
    public struct POVPoint
    {
        public string id;
        public Transform targetTransform;
        public Color color;
        public Sprite icon;
    }

    [SerializeField] private Canvas screenSpaceCanvas;
    [SerializeField] private GameObject markerPrefab; // Should have Image component
    [SerializeField] private float screenPadding = 50f; // Padding from screen edges
    [SerializeField] private float markerSize = 40f;
    [SerializeField] private float maxOffscreenDistance = 200f; // Distance markers stay from edge

    private Dictionary<string, POVPoint> points = new Dictionary<string, POVPoint>();
    private Dictionary<string, RectTransform> markerUIElements = new Dictionary<string, RectTransform>();
    private Camera mainCamera => Camera.allCameras[0];
    private RectTransform canvasRect;
    [SerializeField] private bool showOffscreenMarkers = true;

    private void Start()
    {
        canvasRect = screenSpaceCanvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateAllMarkers();
    }

    /// <summary>
    /// Add a new point of interest to track
    /// </summary>
    public void AddPoint(string id, Transform target, Color color, Sprite icon)
    {
        if (points.ContainsKey(id))
        {
            Debug.LogWarning($"POV Point '{id}' already exists. Removing old one.");
            RemovePoint(id);
        }

        points[id] = new POVPoint
        {
            id = id,
            targetTransform = target,
            color = color,
            icon = icon
        };

        CreateMarkerUI(id);
    }

    /// <summary>
    /// Remove a point of interest
    /// </summary>
    public void RemovePoint(string id)
    {
        if (!points.ContainsKey(id))
        {
            //Debug.LogWarning($"POV Point '{id}' does not exist.");
            return;
        }

        points.Remove(id);

        if (markerUIElements.ContainsKey(id))
        {
            Destroy(markerUIElements[id].gameObject);
            markerUIElements.Remove(id);
        }
    }

    /// <summary>
    /// Update the color of an existing point
    /// </summary>
    public void UpdatePointColor(string id, Color newColor)
    {
        if (!points.ContainsKey(id))
        {
            Debug.LogWarning($"POV Point '{id}' does not exist.");
            return;
        }

        POVPoint point = points[id];
        point.color = newColor;
        points[id] = point;

        if (markerUIElements.ContainsKey(id))
        {
            markerUIElements[id].GetComponent<Image>().color = newColor;
        }
    }

    /// <summary>
    /// Clear all points
    /// </summary>
    public void ClearAllPoints()
    {
        List<string> ids = new List<string>(points.Keys);
        foreach (string id in ids)
        {
            RemovePoint(id);
        }
    }

    private void CreateMarkerUI(string id)
    {
        GameObject markerGO = Instantiate(markerPrefab, transform);
        RectTransform markerRect = markerGO.GetComponent<RectTransform>();

        if (markerRect == null)
        {
            markerRect = markerGO.AddComponent<RectTransform>();
        }

        markerRect.sizeDelta = new Vector2(markerSize, markerSize);

        Image markerImage = markerGO.GetComponent<Image>();
        if (markerImage == null)
        {
            markerImage = markerGO.AddComponent<Image>();
        }

        markerImage.color = points[id].color;
        if (points[id].icon != null)
            markerImage.sprite = points[id].icon;

        markerUIElements[id] = markerRect;
    }

    private void UpdateAllMarkers()
    {
        foreach (var kvp in markerUIElements)
        {
            string id = kvp.Key;
            RectTransform markerRect = kvp.Value;

            if (!points.ContainsKey(id) || points[id].targetTransform == null)
                continue;

            UpdateMarkerPosition(id, markerRect);
        }
    }

    private void UpdateMarkerPosition(string id, RectTransform markerRect)
    {
        Transform target = points[id].targetTransform;
        Vector3 worldPos = target.position;

        // Convert world position to screen position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        bool isOnScreen = screenPos.z > 0 &&
                          screenPos.x > 0 && screenPos.x < mainCamera.pixelWidth &&
                          screenPos.y > 0 && screenPos.y < mainCamera.pixelHeight;

        // Hide off-screen markers if toggle is off
        markerRect.gameObject.SetActive(isOnScreen || showOffscreenMarkers);

        Vector2 uiPos;

        if (isOnScreen)
        {
            // Object is on screen - position directly
            uiPos = RectTransformUtility.PixelAdjustPoint(screenPos, screenSpaceCanvas.transform as RectTransform, screenSpaceCanvas);
            markerRect.position = uiPos;
            markerRect.localScale = Vector3.one;
        }
        else if (showOffscreenMarkers)
        {
            // Object is off screen and markers are visible - clamp to screen edge
            uiPos = ClampToScreenEdge(screenPos);
            markerRect.position = uiPos;
            markerRect.localScale = Vector3.one;
        }
    }

    private Vector2 ClampToScreenEdge(Vector3 screenPos)
    {
        // Normalize screen position relative to screen center
        float screenCenterX = mainCamera.pixelWidth / 2f;
        float screenCenterY = mainCamera.pixelHeight / 2f;

        float dirX = screenPos.x - screenCenterX;
        float dirY = screenPos.y - screenCenterY;

        // Calculate the direction angle
        float angle = Mathf.Atan2(dirY, dirX);

        // Calculate max boundaries with padding
        float maxX = screenCenterX - screenPadding;
        float maxY = screenCenterY - screenPadding;

        // Clamp position to screen edge using the direction
        Vector2 clampedPos;
        float aspectRatio = (float)mainCamera.pixelWidth / mainCamera.pixelHeight;

        // Determine which edge we hit first
        float t = Mathf.Infinity;

        // Check vertical edges (left/right)
        if (Mathf.Abs(dirX) > 0.001f)
        {
            t = Mathf.Min(t, maxX / Mathf.Abs(dirX));
        }

        // Check horizontal edges (top/bottom)
        if (Mathf.Abs(dirY) > 0.001f)
        {
            t = Mathf.Min(t, maxY / Mathf.Abs(dirY));
        }

        clampedPos.x = screenCenterX + dirX * t;
        clampedPos.y = screenCenterY + dirY * t;

        // Apply additional distance offset from edge
        Vector2 edgeDir = clampedPos - new Vector2(screenCenterX, screenCenterY);
        if (edgeDir.magnitude > 0)
        {
            edgeDir.Normalize();
            clampedPos = new Vector2(screenCenterX, screenCenterY) + edgeDir * (edgeDir.magnitude - maxOffscreenDistance * 0.5f);
        }

        return clampedPos;
    }

    /// <summary>
    /// Get the number of tracked points
    /// </summary>
    public int GetPointCount() => points.Count;

    /// <summary>
    /// Check if a point exists
    /// </summary>
    public bool HasPoint(string id) => points.ContainsKey(id);

    /// <summary>
    /// Toggle visibility of off-screen markers
    /// </summary>
    public void SetOffscreenMarkersVisible(bool visible)
    {
        showOffscreenMarkers = visible;
        UpdateOffscreenMarkersVisibility();
    }

    /// <summary>
    /// Toggle visibility of off-screen markers
    /// </summary>
    public void ToggleOffscreenMarkers()
    {
        showOffscreenMarkers = !showOffscreenMarkers;
        UpdateOffscreenMarkersVisibility();
    }

    /// <summary>
    /// Get current state of off-screen marker visibility
    /// </summary>
    public bool AreOffscreenMarkersVisible() => showOffscreenMarkers;

    private void UpdateOffscreenMarkersVisibility()
    {
        foreach (var kvp in markerUIElements)
        {
            string id = kvp.Key;
            RectTransform markerRect = kvp.Value;

            if (!points.ContainsKey(id) || points[id].targetTransform == null)
                continue;

            Transform target = points[id].targetTransform;
            Vector3 worldPos = target.position;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

            bool isOnScreen = screenPos.z > 0 &&
                              screenPos.x > 0 && screenPos.x < mainCamera.pixelWidth &&
                              screenPos.y > 0 && screenPos.y < mainCamera.pixelHeight;

            markerRect.gameObject.SetActive(isOnScreen || showOffscreenMarkers);
        }
    }
}