using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using DXApp_AppData.Model;
using DXApp_AppData.Item;
using System.Collections.Generic;
using Assets.Scripts.Util;
using Assets.Scripts.UI.Common;
using Assets.Scripts.UI.Util;
using Unity.VisualScripting;

namespace Assets.Scripts.UI.Popup.PopupView
{

    public class InventoryView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public ConnectionManager ConnectionManager { get; set; }
        public SoundManager SoundManager { get; set; }


        #region :::::::SerializeFields
        [SerializeField] private AskBreakInView _askBreakInView;
        [SerializeField] private DragAndRotateCharacter _dragAndRotateCharacter;

        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _iconListButton;
        [SerializeField] private Button _explainButton;
        [SerializeField] private Button _favoriteButton;
        [SerializeField] private Button _breakButton;

        [SerializeField] private Text _titleText;

        [SerializeField] private ReuseComponent _originReuseComponent;
        [SerializeField] private ReuseComponent _customReuseComponent;

        [SerializeField] private Toggle _originToggle;
        [SerializeField] private Toggle _combineToggle;

        [SerializeField] private Transform _characterTransform;
        #endregion
        
		public OnEventTrigger<PlayfabItemInstance> OnClick { get; set; } = new OnEventTrigger<PlayfabItemInstance>();
        private List<CustomFigureInstance> CustomFigures { get; set; }
        private List<OriginFigureInstance> OriginFigures { get; set; }

        private const int _horizontalCount = 3; // 가로에 들어가는 컨텐츠 개수
        private const int _defaultSlotCount = 2; // 한개의 컨텐츠에 들어가는 블락수(가로 개수)

        private bool isFirstLoad = false;

        public void Initialize()
        {
            AddEvent();

            if (null != PlayerViewModel.FigureArchive)
            {
                // favorite이 CustomFigure면 CustomFigure 인벤토리를 먼저 켠다
                if (PlayerViewModel.FigureArchive is CustomFigureInstance)
                {
                    _combineToggle.isOn = true;
                    PlayerViewModel.OnFigureArchive.Invoke(true);
                }
                else
                {
                    _originToggle.isOn = true;
                    PlayerViewModel.OnFigureArchive.Invoke(false);
                }
            }
            else
            {
                _originToggle.isOn = true;
                PlayerViewModel.OnFigureArchive.Invoke(false);

                _dragAndRotateCharacter.enabled = false;
            }

            _originReuseComponent.UpdateItem += Origin_UpdateItem;
            _customReuseComponent.UpdateItem += Custom_UpdateItem;

            SetData();
        }

        private void AddEvent()
        {
            #region ::::::ButtonSettings
            _homeButton.OnClickAsObservable("Button_Click").Subscribe(_ => FlowManager.Change(PopupStyle.Lobby)).AddTo(gameObject);
            _explainButton.OnClickAsObservable().Subscribe(_ => 
            {
                if (PlayerViewModel.FigureArchive != null)
                {
                    SoundManager.Play("Button_Click");
                    FlowManager.AddSubPopup(PopupStyle.InventoryExplain);
                }
                else SoundManager.Play("ButtonFail_SFX");
            }).AddTo(gameObject);

            _favoriteButton.OnClickAsObservable().Subscribe(_ =>
            {
                if(null != PlayerViewModel.FigureArchive && PlayerViewModel.Player.FavoriteInstanceID != PlayerViewModel.FigureArchive.InstanceID)
                {
                    ConnectionManager.ChangeFavoriteFigure(PlayerViewModel.FigureArchive.InstanceID);
                    SystemLoading.Show(SystemLoading.LoadingSize.Big,this);
                    SoundManager.Play("Button_Click");
                }
                else
                {
                    SoundManager.Play("ButtonFail_SFX");
                }
            }).AddTo(gameObject);

            PlayerViewModel.ServerRespones.AsObservable().Subscribe(_ =>
            {
                if (null != PlayerViewModel.FigureArchive && PlayerViewModel.Player.FavoriteInstanceID == PlayerViewModel.FigureArchive.InstanceID)
                {
                    _favoriteButton.GetComponent<Image>().color = Color.white;
                }
                else
                {
                    _favoriteButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
                }

                SystemLoading.Hide(this);
            }).AddTo(gameObject);

            PlayerViewModel.OnFigureArchive.AsObservable().Subscribe(data =>
            {
                if (data)
                    _breakButton.gameObject.SetActive(true);
                else
                    _breakButton.gameObject.SetActive(false);
            }).AddTo(gameObject);

            _breakButton.OnClickAsObservable("Button_Click").Subscribe(_ =>
            {
                _askBreakInView.gameObject.SetActive(true);
                _askBreakInView.SetData(_titleText.text);
            }).AddTo(gameObject);
            #endregion

            #region ::::::Toggles
            _originToggle.OnValueChangedAsObservable().Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _originReuseComponent.gameObject.SetActive(true);
                    _customReuseComponent.gameObject.SetActive(false);
                }
            }).AddTo(gameObject);

            _combineToggle.OnValueChangedAsObservable().Subscribe(isOn =>
            {
                SoundManager.Play("Button_Touch");
                if (isOn == true)
                {
                    _originReuseComponent.gameObject.SetActive(false);
                    _customReuseComponent.gameObject.SetActive(true);
                }
            }).AddTo(gameObject);

            #endregion

            #region ::::::ReuseSetting

            _originReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _originReuseComponent.AutoCreateCount; i++)
                {
                    var item = _originReuseComponent.GetContext<ItemInventory_Line>(i);
                    item.OnClick = OnClick;
                    item.Initialize();
                }

            }).AddTo(gameObject);

            _customReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _customReuseComponent.AutoCreateCount; i++)
                {
                    var item = _customReuseComponent.GetContext<ItemInventory_Line>(i);
                    item.OnClick = OnClick;
                    item.Initialize();
                }
            }).AddTo(gameObject);

            OnClick.AsObservable().Subscribe(data =>
            {
                PlayerViewModel.SetData(data);
                _dragAndRotateCharacter.enabled = true;
            }).AddTo(gameObject);
            #endregion

            ResourcesManager.ModelingSetting(_characterTransform);

        }

        #region ::::::::::ReuseUpdate
        private void Origin_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemInventory_Line>();
            var lineCount = item.ItemInventoryBlocks.Length;

            bool[] findFavorite = null;

            if (!isFirstLoad)
            {
                findFavorite = new bool[lineCount];
            }

            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= OriginFigures.Count)
                    continue;
                var data = OriginFigures[RealIndex];
                var isSelect = PlayerViewModel.FigureArchive == data;
                if (!isFirstLoad)  findFavorite[i] = isSelect;
                item.SetData(i, data, isSelect);
            }

            //들어와서 밑에있는 대표피규어를 찾는다
            if (isFirstLoad || false == PlayerViewModel.FigureArchive is OriginFigureInstance) return;

            for (int i = 0; i < findFavorite.Length; i++)
            {
                if (findFavorite[i] == true)
                {
                    isFirstLoad = true;
                    return;
                }
            }

            _originReuseComponent.ComponentParent.transform.localPosition += new Vector3(0, _originReuseComponent.OnePointSize.y, 0);
        }
        private void Custom_UpdateItem(int index, GameObject go)
        {
            var item = go.GetComponent<ItemInventory_Line>();
            var lineCount = item.ItemInventoryBlocks.Length;

            bool[] findFavorite = null;

            if (!isFirstLoad)
            {
                findFavorite = new bool[lineCount];
            }

            for (int i = 0; i < lineCount; i++)
            {
                item.SetEmpty(i);
                int RealIndex = (index * lineCount) + i; //실제 인덱스
                if (RealIndex >= CustomFigures.Count)
                    continue;
                var data = CustomFigures[RealIndex];
                var isSelect = PlayerViewModel.FigureArchive == data;
                if (!isFirstLoad) findFavorite[i] = isSelect;
                item.SetData(i, data, isSelect);
            }

            //들어와서 밑에있는 대표피규어를 찾는다
            if (isFirstLoad || false == PlayerViewModel.FigureArchive is CustomFigureInstance) return;

            for (int i = 0; i < findFavorite.Length; i++)
            {
                if (findFavorite[i] == true)
                {
                    isFirstLoad = true;
                    return;
                }
            }

            _customReuseComponent.ComponentParent.transform.localPosition += new Vector3(0, _customReuseComponent.OnePointSize.y, 0);
        }

        private void SetCount()
        {
            #region ::::::OriginFigures
            var originfigures = OriginFigures;
            int originremainder = originfigures.Count % _horizontalCount;
            if (originfigures.Count <= 6)
            {
                _originReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (originremainder != 0 && originfigures.Count > 6)
            {
                originremainder = 1;
                _originReuseComponent.SetCount((originfigures.Count / _horizontalCount) + originremainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _originReuseComponent.SetCount((originfigures.Count / _horizontalCount));
            }
            #endregion

            #region ::::::CustomFigures
            var customfigures = CustomFigures;
            int customremainder = customfigures.Count % _horizontalCount;
            if (customfigures.Count <= 6)
            {
                _customReuseComponent.SetCount(_defaultSlotCount); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else if (customremainder != 0 && customfigures.Count > 6)
            {
                customremainder = 1;
                _customReuseComponent.SetCount((customfigures.Count / _horizontalCount) + customremainder); // 가로 개수로 나누고 나머지를 더한다(진짜 필요한 개수)
            }
            else
            {
                _customReuseComponent.SetCount((customfigures.Count / _horizontalCount));
            }
            #endregion
        }
        #endregion

        private string SettingTitle()
        {
            if (PlayerViewModel.FigureArchive == null) return null;

            string title = string.Empty;
            var originFigure = PlayerViewModel.FigureArchive as OriginFigureInstance;
            var customFigure = PlayerViewModel.FigureArchive as CustomFigureInstance;
            if (originFigure != null) title = originFigure.Name.FromStringTable();
            else title = customFigure.CustomData.Name;

            return title;
        }

        public void SetData()
        { 
            CustomFigures = PlayerViewModel.Inventory.CustomFigures;
            OriginFigures = PlayerViewModel.Inventory.OriginFigures;

            SetCount();

            _titleText.text = SettingTitle();

            

            #region ::::::::Icon_favorite SetColor
            // 대표 피규어 아이콘 컬러 처리
            if (null != PlayerViewModel.FigureArchive && PlayerViewModel.Player.FavoriteInstanceID == PlayerViewModel.FigureArchive.InstanceID)
            {
                _favoriteButton.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _favoriteButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1);
            }
            #endregion
        }
    }
}
