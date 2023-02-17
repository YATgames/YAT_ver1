using Assets.Scripts.Common.DI;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using UniRx;
using Assets.Scripts.Common.Models;
using System.Linq;
using Assets.Scripts.UI.Common;

namespace Assets.Scripts.UI.Popup.Sub
{
	public class UIPopupCaseInven : PopupSub
	{
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
		[DependuncyInjection(typeof(ConnectionManager))]
		private ConnectionManager _connectionManager;
		[DependuncyInjection(typeof(ItemManager))]
		private ItemManager _itemManager;

		[SerializeField] private Transform _blank;
		[SerializeField] private GameObject _alarmText;
		[SerializeField] private CaseInvenView _caseInventoryView;

        private OnEventTrigger<string> _onClick = new OnEventTrigger<string>();
		private int _caseNumber;

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);

			SetHideCheckTransform(_blank);

			_caseInventoryView.OnClick = _onClick;
			_caseInventoryView.Initialize();
            _caseInventoryView.PlayerViewModel = _playerViewModel;

            if (_playerViewModel.Inventory.Themes.Count == 0) _alarmText.SetActive(true);
            else _alarmText.SetActive(false);

            _playerViewModel.ObserveEveryValueChanged(v => v.Player).Skip(1).Subscribe(p =>
			{
				SystemLoading.Hide(this);
				Hide();
			}).AddTo(gameObject);

			_onClick.AsObservable().Subscribe(themeID =>
			{
				var caseInfo = _playerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == _caseNumber);

				if (caseInfo == null || caseInfo.ThemeID.Equals(themeID))
					return;

				caseInfo.ThemeID = themeID;
				_connectionManager.RegistCase(caseInfo);
				SystemLoading.Show(SystemLoading.LoadingSize.Small, this);
			}).AddTo(gameObject);
		}

		public override void Show(params object[] data)
		{
			base.Show(data);
			if (data.Length < 1 || data[0] is int == false)
			{
				LogManager.Error("UIPopupCaseInven ::: Not Found Data Case Number");
				Hide();
				return;
			}

			_caseNumber = (int)data[0];


			var datas = _playerViewModel.Inventory.Themes.Select(v => _itemManager.GetTheme(v)).ToList();
			_caseInventoryView.SetData(datas, _caseNumber);
		}
	}
}
