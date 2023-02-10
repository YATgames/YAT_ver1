using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Scripts.UI.Util
{
	[RequireComponent(typeof(Image))]
	public class ImageTableComponenet : MonoBehaviour
	{
		private Image _image;

		[SerializeField] private bool _setNativeSize;
		[SerializeField]
		private List<ImageTableMapper> _tables = new List<ImageTableMapper>
		{
			new ImageTableMapper{Language = SystemLanguage.Korean},
			new ImageTableMapper{Language = SystemLanguage.English},
		};

		private void Start()
		{
			GameManager.Instance.CurrentLanguage
				.ObserveEveryValueChanged(v => v.Value)
				.Where(v => _tables.FirstOrDefault(v2 => v == v2.Language) != null)
				.Subscribe(lan =>
				{
					SetData(lan);
				})
				.AddTo(gameObject);
		}

		public void SetData(SystemLanguage lan)
		{
			if (_image == null) _image = GetComponent<Image>();

			_image.sprite = _tables.FirstOrDefault(v => v.Language == lan)?.Sprite;

			if(_setNativeSize) _image.SetNativeSize();
		}
	}

	[Serializable]
	public class ImageTableMapper
	{
		public SystemLanguage Language;
		public Sprite Sprite;
	}
}
