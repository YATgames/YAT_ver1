using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Item;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using DXApp_AppData.Model;
using DXApp_AppData.Enum;
using Assets.Scripts.UI.Util;
using DXApp_AppData.Item;
using System.Collections.Generic;
using DXApp_AppData.Table;
using Assets.Scripts.Util;
using Unity.VisualScripting;
using Assets.Scripts.PrefabModel;
using Assets.Scripts.UI.Common;
using System.Collections;

namespace Assets.Scripts.UI.Popup.PopupView
{

    public class CombineView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public ConfigManager ConfigManager { get; set; }
        public SoundManager SoundManager { get; set; }

        #region ::::::SerializeFields

        [SerializeField] private ModelingEffectController _modelingEffectController;

        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _combineButton;
        [SerializeField] private Button _explainButton;
        [SerializeField] private Button _cancelNickNameButton;

        [SerializeField] private ReuseComponent _headReuseComponent;
        [SerializeField] private ReuseComponent _bodyReuseComponent;
        [SerializeField] private ReuseComponent _decoReuseComponent;
        [SerializeField] private ReuseComponent _propertyReuseComponent;

        [SerializeField] private Toggle _headToggle;
        [SerializeField] private Toggle _bodyToggle;
        [SerializeField] private Toggle _decorationToggle;
        [SerializeField] private Toggle _propertyToggle;

        [SerializeField] private GameObject[] _combineInventories;
        [SerializeField] private ItemCombineTitleWriter _itemCombineTitleWriter;

        [SerializeField] private Transform _characterTransform;
        #endregion

        public OnEventTrigger<PartsInstance> PartsOnClick { get; set; } = new OnEventTrigger<PartsInstance>();
        public OnEventTrigger<FigureTypeTable> PropertyOnClick { get; set; } = new OnEventTrigger<FigureTypeTable>();

        private List<PartsInstance> HeadItems { get; set; }
        private List<PartsInstance> BodyItems { get; set; }
        private List<PartsInstance> DecoItems { get; set; }
        private List<FigureTypeTable> PropertyList { get; set; }

        private const int _horizontalCount = 3; // 가로에 들어가는 컨텐츠 개수
        private const int _defaultSlotCount = 2; // 한개의 컨텐츠에 들어가는 블락수(가로 개수)

        // Start is called before the first frame update
        public void Initialize()
        {
            AddEvent();

            _headReuseComponent.UpdateItem += Head_UpdateItem;
            _bodyReuseComponent.UpdateItem += Body_UpdateItem;
            _decoReuseComponent.UpdateItem += Deco_UpdateItem;
            _propertyReuseComponent.UpdateItem += Property_UpdateItem;

            SetData(PlayerViewModel.Inventory);
        }

        private void AddEvent()
        {
            #region ::::::ButtonSettings
            _homeButton.OnClickAsObservable("Button_Click").Subscribe(_ => FlowManager.Change(PopupStyle.Lobby)).AddTo(gameObject);
            _explainButton.OnClickAsObservable("Button_Click").Subscribe(_ => FlowManager.AddSubPopup(PopupStyle.CombineExplain)).AddTo(gameObject);

            _combineButton.OnClickAsObservable("Button_Click").Subscribe(_ =>
            {
                ActiveInventories(false, true);
                PlayerViewModel.OnPartArchive.Invoke(false);
            }).AddTo(gameObject);
            _cancelNickNameButton.OnClickAsObservable("Button_Click").Subscribe(_ =>
            {
                ActiveInventories(true, false);
                PlayerViewModel.OnPartArchive.Invoke(true);
            }).AddTo(gameObject);

            #endregion

            #region :::::::UpdateItems

            PartsOnClick.AsObservable().Subscribe(data =>
            {
                SoundManager.Play("Touch_Parts");
                PlayerViewModel.SetData(data);
                var model = ResourcesManager.GetModel(data.ID)?.GetComponent<ModelPrefabBase>();
                var itemPartsType = ItemManager.GetPartsCustomData(data).PartsType;
                _modelingEffectController.PlayPartsEffect(itemPartsType, model, PlayerViewModel.PartsArchive);

            }).AddTo(gameObject);

            PropertyOnClick.AsObservable().Subscribe(data =>
            {
                PlayerViewModel.SetData(data);
                _modelingEffectController.PlayPropertyEffect(data.FigureType, PlayerViewModel.FigureTypeTable);

            }).AddTo(gameObject);

            _headReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _headReuseComponent.AutoCreateCount; i++)
                {
                    var item = _headReuseComponent.GetContext<ItemCombine_Line>(i);
                    item.OnClick = PartsOnClick;
                    item.Initialize();
                }
            }).AddTo(gameObject);

            _bodyReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _bodyReuseComponent.AutoCreateCount; i++)
                {
                    var item = _bodyReuseComponent.GetContext<ItemCombine_Line>(i);
                    item.OnClick = PartsOnClick;
                    item.Initialize();
                }
            }).AddTo(gameObject);

            _decoReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _decoReuseComponent.AutoCreateCount; i++)
                {
                    var item = _decoReuseComponent.GetContext<ItemCombine_Line>(i);
                    item.OnClick = PartsOnClick;
                    item.Initialize();
                }
            }).AddTo(gameObject);

            _propertyReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _propertyReuseComponent.AutoCreateCount; i++)
                {
                    var item = _propertyReuseComponent.GetContext<ItemProperty_Line>(i);
                    item.OnClick = PropertyOnClick;
                    item.Initialize();
                }
            }).AddTo(gameObject);

            #endregion

            TogleEventSettings();

            //합체 버튼 온오프
            PlayerViewModel.OnPartArchive.AsObservable().Subscribe(data =>
            {
                if (data)
                    _combineButton.gameObject.SetActive(true);
                else
                    _combineButton.gameObject.SetActive(false);
            }).AddTo(gameObject);

            ResourcesManager.ModelingSetting(_characterTransform);
        }

        #region :::::::ReuseUpdate
        private void Head_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemCombine_Line>();
            var lineCount = item.ItemCombineBlocks.Length;
            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= HeadItems.Count)
                    continue;
                var data = HeadItems[RealIndex];

                bool isSelect = false;
                if (PlayerViewModel.PartsArchive?.FirstOrDefault(v => v.InstanceID == data.InstanceID) == null) isSelect = false;
                else isSelect = true;
                item.SetData(i, data, isSelect);
            }
        }
        private void Body_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemCombine_Line>();
            var lineCount = item.ItemCombineBlocks.Length;
            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= BodyItems.Count)
                    continue;
                var data = BodyItems[RealIndex];

                bool isSelect = false;
                if (PlayerViewModel.PartsArchive?.FirstOrDefault(v => v.InstanceID == data.InstanceID) == null) isSelect = false;
                else isSelect = true;
                item.SetData(i, data, isSelect);
            }
        }
        private void Deco_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemCombine_Line>();
            var lineCount = item.ItemCombineBlocks.Length;
            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= DecoItems.Count)
                    continue;
                var data = DecoItems[RealIndex];

                bool isSelect = false;
                if (PlayerViewModel.PartsArchive?.FirstOrDefault(v => v.InstanceID == data.InstanceID) == null) isSelect = false;
                else isSelect = true;
                item.SetData(i, data, isSelect);
            }
        }
        private void Property_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemProperty_Line>();
            var lineCount = item.ItemPropertyBlocks.Length;
            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= PropertyList.Count)
                    continue;
                var data = PropertyList[RealIndex];

                bool isSelect;
                if (PlayerViewModel.FigureTypeTable != data) isSelect = false;
                else isSelect = true;
                item.SetData(i, data, isSelect);
            }
        }
        private void SetCount()
        {
            #region :::::::HeadItem
            var headItems = HeadItems;
            int headRemainder = headItems.Count % _horizontalCount;
            if (headItems.Count <= 6)
            {
                _headReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (headRemainder != 0 && headItems.Count > 6)
            {
                headRemainder = 1;
                _headReuseComponent.SetCount((headItems.Count / _horizontalCount) + headRemainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _headReuseComponent.SetCount((headItems.Count / _horizontalCount));
            }
            #endregion

            #region :::::::BodyItem
            var bodyitems = BodyItems;
            int bodyRemainder = bodyitems.Count % _horizontalCount;
            if (headItems.Count <= 6)
            {
                _bodyReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (bodyRemainder != 0 && bodyitems.Count > 6)
            {
                bodyRemainder = 1;
                _bodyReuseComponent.SetCount((bodyitems.Count / _horizontalCount) + bodyRemainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _bodyReuseComponent.SetCount((bodyitems.Count / _horizontalCount));
            }
            #endregion

            #region :::::::DecoItem
            var decoItems = DecoItems;
            int decoRemainder = decoItems.Count % _horizontalCount;
            if (decoItems.Count <= 6)
            {
                _decoReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (decoRemainder != 0 && decoItems.Count > 6)
            {
                decoRemainder = 1;
                _decoReuseComponent.SetCount((decoItems.Count / _horizontalCount) + decoRemainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _decoReuseComponent.SetCount((decoItems.Count / _horizontalCount));
            }
            #endregion

            #region :::::::Propertys
            var propertyList = PropertyList;
            int propertyRemainder = propertyList.Count % _horizontalCount;
            if (propertyList.Count <= 6)
            {
                _propertyReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (propertyRemainder != 0 && propertyList.Count > 6)
            {
                propertyRemainder = 1;
                _propertyReuseComponent.SetCount((propertyList.Count / _horizontalCount) + propertyRemainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _propertyReuseComponent.SetCount((propertyList.Count / _horizontalCount));
            }
            #endregion
        }
        #endregion

        private void TogleEventSettings()
        {
            _headToggle.OnValueChangedAsObservable().Skip(1).Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _headReuseComponent.gameObject.SetActive(true);
                    _bodyReuseComponent.gameObject.SetActive(false);
                    _decoReuseComponent.gameObject.SetActive(false);
                    _propertyReuseComponent.gameObject.SetActive(false);
                }
            }).AddTo(gameObject);

            _bodyToggle.OnValueChangedAsObservable().Skip(1).Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _headReuseComponent.gameObject.SetActive(false);
                    _bodyReuseComponent.gameObject.SetActive(true);
                    _decoReuseComponent.gameObject.SetActive(false);
                    _propertyReuseComponent.gameObject.SetActive(false);
                }
            }).AddTo(gameObject);

            _decorationToggle.OnValueChangedAsObservable().Skip(1).Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _headReuseComponent.gameObject.SetActive(false);
                    _bodyReuseComponent.gameObject.SetActive(false);
                    _decoReuseComponent.gameObject.SetActive(true);
                    _propertyReuseComponent.gameObject.SetActive(false);
                }
            }).AddTo(gameObject);

            _propertyToggle.OnValueChangedAsObservable().Skip(1).Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _headReuseComponent.gameObject.SetActive(false);
                    _bodyReuseComponent.gameObject.SetActive(false);
                    _decoReuseComponent.gameObject.SetActive(false);
                    _propertyReuseComponent.gameObject.SetActive(true);
                }
            }).AddTo(gameObject);
        }

        private void ActiveInventories(bool combineInventoriesBool, bool combineTitleWriterBool)
        {
            for (int i = 0; i < _combineInventories.Length; i++)
            {
                _combineInventories[i].SetActive(combineInventoriesBool);
            }
            _itemCombineTitleWriter.gameObject.SetActive(combineTitleWriterBool);
        }

        public void SetData(InventoryModel datas)
        {
            ActiveInventories(true, false);

            HeadItems = datas.PartsList.Where(v => v.ID.Substring(0,1) == "1").ToList();
            BodyItems = datas.PartsList.Where(v => v.ID.Substring(0, 1) == "2").ToList();
            DecoItems = datas.PartsList.Where(v => v.ID.Substring(0, 1) == "3" || v.ID.Substring(0, 1) == "4").ToList();
            PropertyList = ConfigManager.FigureTypes.Where(v => v.FigureType != FigureType.None).ToList();

            SetCount();
        }
    }
}
