using Assets.Scripts.Common.DI;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;

namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupInventoryExplain : PopupSub
    {
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;
        [DependuncyInjection(typeof(ConfigManager))]
        private ConfigManager _configManager;

        [SerializeField] private Button _closeButton;

        [SerializeField] private InventoryExplainView _inventoryExplainView;
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _inventoryExplainView.ItemManager = _itemManager;
            _inventoryExplainView.ConfigManager = _configManager;

            _inventoryExplainView.SetData(_playerViewModel.FigureArchive);
        }
        public void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            _closeButton.OnPointerDownAsObservable().Subscribe(_ => Hide()).AddTo(gameObject);
        }
        public override void Show(params object[] data)
        {
            base.Show(data);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
