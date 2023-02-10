using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
{
    public class ItemProperty_Block : MonoBehaviour
    {
        private enum BgSpritesType { On, Off, Empty }

        [SerializeField] private Button _selectButton;

        [SerializeField] private Image _selectButtonImg;

        [SerializeField] private Image _bgFrame;

        [SerializeField] private Image[] _ghostImages;

        [SerializeField] private Sprite[] _bgSprites;

        public OnEventTrigger<FigureTypeTable> OnClick;

        private bool _firstCheck = false;

        private RectTransform[] _ghostImgRectTransforms;

        private FigureTypeTable _figureTypeTable;

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _selectButton.OnClickAsObservable().Subscribe(_ =>
            {
                if(_figureTypeTable != null && _bgFrame.sprite != _bgSprites[(int)BgSpritesType.Off])
                {
                    OnClick?.Invoke(_figureTypeTable);
                }
            }).AddTo(gameObject);
        }
       
        public void SetData(FigureTypeTable data, bool isSelect)
        {
            #region ::::::GetRectTransform
            if (!_firstCheck)
            {
                _ghostImgRectTransforms = new RectTransform[_ghostImages.Length];

                for (int i = 0; i < _ghostImages.Length; i++)
                {
                    _ghostImgRectTransforms[i] = _ghostImages[i].GetComponent<RectTransform>();
                }
                _firstCheck = true;
            }
            #endregion

            #region ::::::SetData

            // 이미지 세팅
            for (int i = 0; i < _ghostImages.Length; i++)
            {
                if (i == 0)
                {
                    _ghostImages[i].sprite = ResourcesManager.GetImages(data.FigureType.ToString());
                }
                else
                {
                    _ghostImages[i].sprite = _bgSprites[(int)BgSpritesType.Empty];
                }
            }
            _bgFrame.sprite = _bgSprites[(int)BgSpritesType.On];
            #endregion

            _selectButtonImg.gameObject.SetActive(isSelect);

            _figureTypeTable = data;
        }

        public void SetEmpty()
        {
            foreach (var item in _ghostImages)
            {
                item.sprite = _bgSprites[(int)BgSpritesType.Empty];
            }

            _selectButtonImg.gameObject.SetActive(false);

            _bgFrame.sprite = _bgSprites[(int)BgSpritesType.Off];
        }

    }
}
