//using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
    public class AutoScaler : MonoBehaviour
    {
        [SerializeField] private CanvasScaler canvasScaler;
        private void Start()
        {
            //var canvasScaler = ObjectFinder.FindParent<CanvasScaler>(transform);
            if (canvasScaler == null) return;
            var canvasRect = canvasScaler.GetComponent<RectTransform>();
            if (canvasRect == null) return;


            var keepScale = transform.localScale;
            // ĵ���� �����Ϸ����� ���� �������� scale �����ϱ⶧���� ���� ������ ������� �����ϰ� ����
            // 720 x 1280 ���� / �������� 580 x 1280 --> 580 / 720 ������� ����
            float sizeMultiple = 1f;
            if (canvasRect.sizeDelta.x > canvasRect.sizeDelta.y)
            {
                sizeMultiple = canvasRect.sizeDelta.y / canvasScaler.referenceResolution.y;
            }
            else
            {
                sizeMultiple = canvasRect.sizeDelta.x / canvasScaler.referenceResolution.x;
            }
            transform.localScale = new Vector3(keepScale.x * sizeMultiple, keepScale.y * sizeMultiple, transform.localScale.z);
        }
    }
}
