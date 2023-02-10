using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DXApp_AppData.Enum;
using System.Linq;
using Assets.Scripts.PrefabModel;

namespace Assets.Scripts.UI.Item
{
    public class ItemInventory_Block : MonoBehaviour
    {
        public ConfigManager ConfigManager { get; set; }
        private enum BgSpritesType { On, Off, Empty }

        [SerializeField] private Button _selectButton;

        [SerializeField] private Image _selectButtonImg;
        [SerializeField] private Image _bgFrame;

        [SerializeField] private Image _preDecoImage;
        [SerializeField] private Image _bodyImage;
        [SerializeField] private Image _headImage;
        [SerializeField] private Image _postDecoImage;

        [SerializeField] private Sprite[] _bgSprites;

        public OnEventTrigger<PlayfabItemInstance> OnClick;

        private PlayfabItemInstance _data;

        private bool isSelected;

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _selectButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (_data != null && !isSelected && _bgFrame.sprite != _bgSprites[(int)BgSpritesType.Off])
                {
                    OnClick?.Invoke(_data);
                }
            }).AddTo(gameObject);
        }

        private void ResetModels()
        {
            for (int i = 0; i < ItemManager.Instance.PartsList.Count; i++)
            {
                ResourcesManager.ResetModels(ItemManager.Instance.PartsList[i].ID);
            }
        }

        public void SetData(OriginFigureInstance data, bool isSelect)
        {
            SetEmpty();

            #region ::::::SettingImages
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
            _bgFrame.sprite = _bgSprites[(int)BgSpritesType.On];
            #endregion

            isSelected = isSelect;
            _selectButtonImg.gameObject.SetActive(isSelected);

            if (isSelected)
            {
                ResetModels();

                var items = ItemManager.Instance.Figures.FirstOrDefault(v => v.ID == data.ID).Items;

                GameObject body = null;
                GameObject head = null;
                GameObject deco = null;

                for (int i = 0; i < items.Count; i++)
                {
                    /// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
                    var item = items[i];
                    switch (item.Substring(0, 1))
                    {
                        case "1": head = ResourcesManager.GetModel(item); break;
                        case "2": body = ResourcesManager.GetModel(item); break;
                        case "3": deco = ResourcesManager.GetModel(item); break;
                        case "4": deco = ResourcesManager.GetModel(item); break;
                    }
                }

                Model_Body _body = body?.GetComponent<Model_Body>();
                Model_Head _head = head?.GetComponent<Model_Head>();
                Model_Deco _deco = deco?.GetComponent<Model_Deco>();

                try
                {
                    _body.ResetTransform();
                    _head?.gameObject.transform.SetParent(_body.HeadPosition);
                    _head?.ResetTransform();
                    switch (_deco.DecoType)
                    {
                        case DecoType.OverHead:
                            _deco.gameObject.transform.SetParent(_head.DecoPosition);
                            break;

                        case DecoType.Wing:
                            _deco.gameObject.transform.SetParent(_body.WingPos);
                            break;

                        case DecoType.Tail:
                            _deco.gameObject.transform.SetParent(_body.TailPos);
                            break;
                    }

                    _deco.ResetTransform();
                }
                catch
                {

                }
            }

            _data = data;
        }
        public void SetData(CustomFigureInstance data, bool isSelect)
        {
            SetEmpty();

            #region ::::::SettingImages
            var ps = data.CustomData.Parts;
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
            _bgFrame.sprite = _bgSprites[(int)BgSpritesType.On];
            #endregion

            isSelected = isSelect;
            _selectButtonImg.gameObject.SetActive(isSelected);

            if (isSelected)
            {
                ResetModels();

                var items = data.CustomData.Parts;

                GameObject body = null;
                GameObject head = null;
                GameObject deco = null;

                for (int i = 0; i < items.Count; i++)
                {
                    /// 1 - Head , 2 - Body , 3 - PostDeco , 4 - PreDeco
                    var item = items[i];
                    switch (item.Substring(0, 1))
                    {
                        case "1": head = ResourcesManager.GetModel(item); break;
                        case "2": body = ResourcesManager.GetModel(item); break;
                        case "3": deco = ResourcesManager.GetModel(item); break;
                        case "4": deco = ResourcesManager.GetModel(item); break;
                    }
                }

                Model_Body _body = body?.GetComponent<Model_Body>();
                Model_Head _head = head?.GetComponent<Model_Head>();
                Model_Deco _deco = deco?.GetComponent<Model_Deco>();

               
                try
                {
                    _body.ResetTransform();
                    _head?.gameObject.transform.SetParent(_body.HeadPosition);
                    _head?.ResetTransform();

                    switch (_deco.DecoType)
                    {
                        case DecoType.OverHead:
                            _deco.gameObject.transform.SetParent(_head.DecoPosition);
                            break;

                        case DecoType.Wing:
                            _deco.gameObject.transform.SetParent(_body.WingPos);
                            break;

                        case DecoType.Tail:
                            _deco.gameObject.transform.SetParent(_body.TailPos);
                            break;
                    }

                    _deco.ResetTransform();
                }
                catch
                {

                }
            }

            _data = data;
        }
        public void SetEmpty()
        {
            _headImage.sprite = _bgSprites[(int)BgSpritesType.Empty];
            _bodyImage.sprite = _bgSprites[(int)BgSpritesType.Empty];
            _preDecoImage.sprite = _bgSprites[(int)BgSpritesType.Empty];
            _postDecoImage.sprite = _bgSprites[(int)BgSpritesType.Empty];

            _selectButtonImg.gameObject.SetActive(false);

            _bgFrame.sprite = _bgSprites[(int)BgSpritesType.Off];
        }

    }
}
