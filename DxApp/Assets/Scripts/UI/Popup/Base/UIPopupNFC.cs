using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.Base
{
	public class UIPopupNFC : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
		[DependuncyInjection(typeof(ConnectionManager))]
		private ConnectionManager _connectionManager;
		[DependuncyInjection(typeof(ItemManager))]
		private ItemManager _itemMnanager;
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;

        [SerializeField] private Button _homeButton;

		[SerializeField] private NFCView _nfcView;

		public override void Initialize()
        {
            base.Initialize();
			DependuncyInjection.Inject(this);
			_nfcView.ConnectionManager = _connectionManager;
            _nfcView.PlayerViewModel = _playerViewModel;

            _homeButton.OnClickAsObservable().Subscribe(_ => _flowManager.Change(PopupStyle.Lobby)).AddTo(gameObject);

            _nfcView.SetData(_itemMnanager.Figures);
            _nfcView.SetData(_itemMnanager.Doggabies);
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
