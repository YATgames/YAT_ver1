using Assets.Scripts.Common.DI;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using UniRx;
using Assets.Scripts.UI.Common;
using System;
using DXApp_AppData.Util;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupCombine : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourceManager;
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ConfigManager))]
        private ConfigManager _configManager;
        [DependuncyInjection(typeof(ConnectionManager))]
        private ConnectionManager _connectionManager;
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;


        [SerializeField] private CombineView _combineView;
        public override void Initialize()
        {
            DependuncyInjection.Inject(this);
            base.Initialize();

            _combineView.FlowManager = _flowManager;
            _combineView.ResourcesManager = _resourceManager;
            _combineView.PlayerViewModel = _playerViewModel;
            _combineView.ConfigManager = _configManager;
            _combineView.SoundManager = _soundManager;

            _combineView.Initialize();

            _playerViewModel.ServerRespones.AsObservable().Subscribe(_ => SystemLoading.Hide(this)).AddTo(gameObject);

            _playerViewModel.UpdateItem.
               AsObservable().
               Subscribe(_ => _combineView.SetData(_playerViewModel.Inventory)).
               AddTo(gameObject);

            _soundManager.PlayBGM("Combine_BGM");
            _soundManager.Play("Combine_SFX", true);

            DailyCheck();
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
        }

        public override void Hide()
        {
            base.Hide();
            _soundManager.Stop();
            _playerViewModel.Reset();
        }

        private void DailyCheck()
        {
            DateTime t = _playerViewModel.Account.LastLoginTime == null ? DateTime.MinValue : _playerViewModel.Account.LastLoginTime.Value;

            var DoggabiFigureCount = _playerViewModel.Inventory.Doggabis.Count;
            if (DoggabiFigureCount == 1 && t.DayReset())
            {
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
                _connectionManager.DailyCheck();
            }
        }
    }
}
