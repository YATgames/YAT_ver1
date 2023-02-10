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
    public class UIPopupCombineExplain : PopupSub
    {
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;
        [DependuncyInjection(typeof(ConfigManager))]
        private ConfigManager _configManager;

        [SerializeField] private Button _closeButton;

        [SerializeField] private CombineExplainView _combineExplainView;
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

        }
        public void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            _closeButton.OnClickAsObservable().Select(_ => _closeButton.UpdateAsObservable()).Take(1).Subscribe(_ => Hide()).AddTo(gameObject);
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
