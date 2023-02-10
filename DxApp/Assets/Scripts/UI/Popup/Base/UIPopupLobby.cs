using Assets.Scripts.Common.DI;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using System;
using DXApp_AppData.Util;
using Assets.Scripts.UI.Common;
using UniRx;
using UnityEngine.UI;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI.Popup.Base
{
	public class UIPopupLobby : PopupBase
	{
		[DependuncyInjection(typeof(FlowManager))]
		private FlowManager _flowManager;
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ConnectionManager))]
        private ConnectionManager _connectionManager;
		[DependuncyInjection(typeof(SoundManager))]
		private SoundManager _soundManager;
		[DependuncyInjection(typeof(ResourcesManager))]
		private ResourcesManager _resourcesManager;

		[SerializeField] private LobbyView _lobbyView;

        public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);

			_lobbyView.FlowManager = _flowManager;
			_lobbyView.PlayerViewModel = _playerViewModel;
			_lobbyView.ResourcesManager = _resourcesManager; // 리소스매니저 추가
			_lobbyView.SetData(_playerViewModel.Player.CaseList);

            _playerViewModel.ServerRespones.AsObservable().Subscribe(v => SystemLoading.Hide(this)).AddTo(gameObject);

			_soundManager.PlayBGM("Main_BGM");

            DailyCheck();
        }

		public override void Show(params object[] data)
		{
			base.Show(data);
		}

		public override void Hide()
		{
			base.Hide();
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
