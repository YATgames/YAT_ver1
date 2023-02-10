using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
	public class ItemDisplayInven_Block : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private Sprite _emptySprite;
		[SerializeField] private Image _frameImage;
		[SerializeField] private Image _headImage;
        [SerializeField] private Image _bodyImage;
		[SerializeField] private Image _preDecoImage;
		[SerializeField] private Image _postDecoImage;

		[SerializeField] private Image _selectedImage;

		[SerializeField] private Sprite[] _frameSprites;

		public OnEventTrigger<PlayfabItemInstance> OnClick { get; set; }
		private PlayfabItemInstance _data;

		private void Start()
		{
			if (_button == null)
				return;

			_button.onClick.AsObservable().Subscribe(_ =>
			{
				if (_data == null)
					return;

				OnClick.Invoke(_data);
			}).AddTo(gameObject);
		}

		public void SetEmpty()
		{
			_headImage.sprite = _emptySprite;
			_bodyImage.sprite = _emptySprite;
			_preDecoImage.sprite = _emptySprite;
			_postDecoImage.sprite = _emptySprite;
			_frameImage.sprite = _frameSprites[0];
			_selectedImage.gameObject.SetActive(false);
            _data = null;
		}

		public void SetData(CustomFigureInstance data, bool isSelected)
		{
			SetEmpty();

			var ps = data.CustomData.Parts;
			for (int i = 0; i < ps.Count; i++)
			{
				var p = ps[i];
				/// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
				switch(p.Substring(0 , 1))
				{
					case "1": _headImage.sprite = ResourcesManager.GetImages(p); break;
					case "2": _bodyImage.sprite = ResourcesManager.GetImages(p); break;
					case "3": _postDecoImage.sprite = ResourcesManager.GetImages(p); break;
					case "4": _preDecoImage.sprite = ResourcesManager.GetImages(p); break;
				}
			}
            _frameImage.sprite = _frameSprites[1];
			_selectedImage.gameObject.SetActive(isSelected);
            _data = data;
		}

		public void SetData(OriginFigureInstance data, bool isSelected)
		{
			SetEmpty();

			var ps = ItemManager.Instance.Figures.FirstOrDefault(v => v.ID == data.ID).Items;
			for (int i = 0; i < ps.Count; i++)
			{
				var p = ps[i];
				/// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
				switch (p.Substring(0, 1))
				{
					case "1": _headImage.sprite = ResourcesManager.GetImages(p); break;
					case "2": _bodyImage.sprite = ResourcesManager.GetImages(p); break;
					case "3": _postDecoImage.sprite = ResourcesManager.GetImages(p); break;
					case "4": _preDecoImage.sprite = ResourcesManager.GetImages(p); break;
				}
			}
			_frameImage.sprite = _frameSprites[1];
            _selectedImage.gameObject.SetActive(isSelected);

            _data = data;
        }
	}
}
