using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
	public class AutoScaler : MonoBehaviour
	{
		private void Start()
		{
			var canvasScaler = ObjectFinder.FindParent<CanvasScaler>(transform);
			if (canvasScaler == null) return;
			var canvasRect = canvasScaler.GetComponent<RectTransform>();
			if (canvasRect == null) return;


			var keepScale = transform.localScale;
			// 캔버스 스케일러에서 세로 기준으로 scale 조정하기때문에 가로 사이즈 계산으로 스케일값 조정
			// 720 x 1280 원본 / 리사이즈 580 x 1280 --> 580 / 720 사이즈로 조정
			float sizeMultiple = 1f;
			if(canvasRect.sizeDelta.x > canvasRect.sizeDelta.y)
			{
				sizeMultiple = canvasRect.sizeDelta.y / canvasScaler.referenceResolution.y;
			}
			else
			{
				sizeMultiple = canvasRect.sizeDelta.x / canvasScaler.referenceResolution.x;
			}
			transform.localScale = new Vector3(keepScale.x * sizeMultiple, keepScale.y * sizeMultiple, keepScale.z * sizeMultiple);
		}
	}
}
