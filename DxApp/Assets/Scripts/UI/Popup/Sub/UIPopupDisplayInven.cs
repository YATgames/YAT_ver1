using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using UniRx;
using System.Linq;
using Assets.Scripts.UI.Common;

namespace Assets.Scripts.UI.Popup.Sub
{
	public class UIPopupDisplayInven : PopupSub
	{
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
		[DependuncyInjection(typeof(ConnectionManager))]
		private ConnectionManager _connectionManager;

		[SerializeField] private Transform _blank;
		[SerializeField] private GameObject _alarmText;
		[SerializeField] private UIPopupDisplayInvenView _inventoryView;

		private OnEventTrigger<PlayfabItemInstance> _onClick = new OnEventTrigger<PlayfabItemInstance>();
		private int _caseNumber;

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);

			SetHideCheckTransform(_blank);

			_inventoryView.PlayerViewModel = _playerViewModel;
			_inventoryView.OnClick = _onClick;
			_inventoryView.Initialize();

			if (_playerViewModel.Inventory.CustomFigures.Count == 0 && _playerViewModel.Inventory.OriginFigures.Count == 0) _alarmText.SetActive(true);
			else _alarmText.SetActive(false);

            _playerViewModel.ObserveEveryValueChanged(v => v.Player).Skip(1).Subscribe(p =>
			{
				SystemLoading.Hide(this);
				Hide();
			}).AddTo(gameObject);

			_onClick.AsObservable().Subscribe(item =>
			{
				var caseInfo = _playerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == _caseNumber);
				var usedCase = _playerViewModel.Player.CaseList.FirstOrDefault(v => v.FigureInstanceID.Equals(item.InstanceID));

				if (usedCase != null)
				{
					LogManager.Log("Is Used Figure ::::: Case Number {0}", usedCase.Number);
					return;
				}

				if (caseInfo == null || caseInfo.FigureInstanceID.Equals(item.InstanceID))
					return;

				caseInfo.FigureInstanceID = item.InstanceID;
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

			_inventoryView.SetData(_caseNumber);
		}
	}
}
