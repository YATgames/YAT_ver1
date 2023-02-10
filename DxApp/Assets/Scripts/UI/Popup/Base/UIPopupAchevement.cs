using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupAchevement : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ConfigManager))]
        private ConfigManager _configManager;
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;

        [SerializeField] private AchievemnetView _achievemnetView;


        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _achievemnetView.FlowManager = _flowManager;
            _achievemnetView.PlayerViewModel = _playerViewModel;
            _achievemnetView.ItemManager = _itemManager;

            _achievemnetView.SetData(_configManager.Quests);

            _playerViewModel.ServerRespones.AsObservable().Subscribe(_ =>
            {
                _achievemnetView.SetData(_configManager.Quests);
            }).AddTo(gameObject);
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
