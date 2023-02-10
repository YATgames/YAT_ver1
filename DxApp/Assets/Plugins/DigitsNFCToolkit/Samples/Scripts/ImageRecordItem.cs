using UnityEngine;
using UnityEngine.UI;

namespace DigitsNFCToolkit.Samples
{
	public class ImageRecordItem: RecordItem
	{
		protected Image imageRenderer;

		protected override void Awake()
		{
			base.Awake();
			imageRenderer = transform.Find("Image").GetComponent<Image>();
		}

		public void LoadImage(byte[] bytes)
		{
			Texture2D image = new Texture2D(1, 1, TextureFormat.RGB24, false);
			image.LoadImage(bytes);
			image.filterMode = FilterMode.Point;

			Vector2 pivot = new Vector2(0.5f, 0.5f);
			Rect rect = new Rect(0, 0, image.width, image.height);
			Sprite sprite = Sprite.Create(image, rect, pivot);
			imageRenderer.sprite = sprite;

		}
	}
}