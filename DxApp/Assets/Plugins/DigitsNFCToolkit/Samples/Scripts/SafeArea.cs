using UnityEngine;

namespace DigitsNFCToolkit.Samples
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea: MonoBehaviour
    {
        private RectTransform rectTransform;
        private Rect lastSafeArea;
        private bool initialized;

        private void Awake()
        {
            if(!initialized)
            { Initialize(); }
        }

        private void Initialize()
        {
            rectTransform = GetComponent<RectTransform>();
            lastSafeArea = new Rect(0, 0, 0, 0);

            initialized = true;
        }

        private void OnRectTransformDimensionsChange()
        {
            if(!initialized)
            { Initialize(); }

            Rect safeArea = Screen.safeArea;
            if(lastSafeArea != safeArea)
            {
                ApplySafeArea(safeArea);
            }
        }

        private void ApplySafeArea(Rect rect)
        {
            lastSafeArea = rect;

            Vector2 anchorMin = rect.position;
            Vector2 anchorMax = rect.position + rect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}

