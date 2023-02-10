using UnityEngine;
using UnityEngine.UI;

namespace DigitsNFCToolkit.Samples
{
	[RequireComponent(typeof(RectTransform))]
	public class RecordItem: MonoBehaviour
	{
		public RectTransform RectTransform { get; private set; }

		protected Text label;

		protected virtual void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			label = GetComponentInChildren<Text>();
		}

		public void UpdateLabel(string text)
		{
			label.text = text;
		}
	}
}
