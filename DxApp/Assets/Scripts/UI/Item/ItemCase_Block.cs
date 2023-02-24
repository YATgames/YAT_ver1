using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
	public class ItemCase_Block : MonoBehaviour
	{
		[SerializeField] private Image _temaImage;
		[SerializeField] private Image _frameImage;
		[SerializeField] private Image _selectedImage;
		[SerializeField] private Button _button;

        [SerializeField] private Sprite[] _frameSprites;

		public OnEventTrigger<string> OnClick { get; set; }
		private Theme _theme;

		private void Start()
		{
			_button.OnClickAsObservable("Button_Touch").Subscribe(_ =>
			{
				OnClick.Invoke(_theme.ID);
			}).AddTo(gameObject);
		}

		public void SetActiveImage(bool v)
		{
			_temaImage.gameObject.SetActive(v);

			if (v == true) _frameImage.sprite = _frameSprites[1];
			else _frameImage.sprite = _frameSprites[0];

        }

		public void SetData(Theme theme, bool isSelected)
		{
			_temaImage.sprite = ResourcesManager.GetTheme(theme);
			_selectedImage.gameObject.SetActive(isSelected);
            _theme = theme;
		}
	}
}