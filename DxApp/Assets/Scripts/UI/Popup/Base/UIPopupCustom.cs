using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using UniRx;
using Assets.Scripts.Util;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.Base
{
	public class UIPopupCustom : PopupBase
	{
		[DependuncyInjection(typeof(FlowManager))]
		private FlowManager _flowManager;
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourceManager;
        [DependuncyInjection(typeof(ConnectionManager))]
        private ConnectionManager _connectionManager;

        [SerializeField] private CustomView _customView;
		[SerializeField] private Button _closeButton;

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);
			_customView.FlowManager = _flowManager;
			_customView.PlayerViewModel = _playerViewModel;
            _customView.ResourcesManager = _resourceManager;
            _customView.ConnectionManager = _connectionManager;

            _closeButton.OnClickAsObservable().Subscribe(_ => _flowManager.Change(PopupStyle.Lobby)).AddTo(gameObject);
		}

		public override void Show(params object[] data)
		{
			base.Show(data);
			if (data.Length == 0 || data[0] is int == false)
			{
				Hide();
				LogManager.Error("UIPopupCustom :: Datas Check !!");
				return;
			}

			var caseNumber = (int)data[0];
			_customView.CaseNumber = caseNumber;
			_customView.Initialize();
			_playerViewModel.ObserveEveryValueChanged(v => v.Player).Subscribe(p =>
			{
                var caseInfo = p.CaseList.FirstOrDefault(v => v.Number == _customView.CaseNumber);

				if (caseInfo == null) return;

				_customView.SetData(caseInfo);
			}).AddTo(gameObject);
		}
	}
}
