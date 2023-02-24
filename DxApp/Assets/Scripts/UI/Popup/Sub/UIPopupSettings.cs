using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using DXApp_AppData.Model;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI.Popup.Sub
{
	public class UIPopupSettings : PopupSub
	{
		[DependuncyInjection(typeof(PlayerViewModel))]
		private PlayerViewModel _playerViewModel;
		[DependuncyInjection(typeof(FlowManager))]
		private FlowManager _flowManager;
		[DependuncyInjection(typeof(GameManager))]
		private GameManager _gameManager;

		[SerializeField] private SettingsView _settingsView;
		[SerializeField] private Button _closeButton;

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);

			_settingsView.GameManager = _gameManager;
			_settingsView.SetNickName(_playerViewModel.Account.NickName);
			_settingsView.SetRanking();

			var favoriteFigure_origin = _playerViewModel.Inventory.OriginFigures.FirstOrDefault(v => v.InstanceID == _playerViewModel.Player.FavoriteInstanceID);
			var favoriteFigure_custom = _playerViewModel.Inventory.CustomFigures.FirstOrDefault(v => v.InstanceID == _playerViewModel.Player.FavoriteInstanceID);

			if (favoriteFigure_origin != null) _settingsView.SetFavoriteFigure(favoriteFigure_origin);
			else if (favoriteFigure_custom != null) _settingsView.SetFavoriteFigure(favoriteFigure_custom);
			else _settingsView.SetEmptyFavoriteFigure();

			var figureCount = _playerViewModel.Inventory.CustomFigures.Count + _playerViewModel.Inventory.OriginFigures.Count;
			_settingsView.SetFigureCount(figureCount);
			

			//_nickNameField.ObserveEveryValueChanged(v => v.isFocused).Skip(1).Where(v => v == false).Subscribe(_ =>
			//{
			//    if (string.IsNullOrEmpty(_nickNameField.text) == true)
			//        _nickNameField.text = _keepNickName;
			//    else
			//    {
			//        //LogManager.KeepServer("SettingsView :: 닉네임 변경 {0}", _nickNameField.text);
			//        _nickNameField.text = _keepNickName;
			//    }
			//}).AddTo(gameObject);

			_closeButton.OnClickAsObservable("Button_Click").Subscribe(_ => Hide()).AddTo(gameObject);
		}

		public override void Show(params object[] data)
		{
			base.Show(data);
		}
	}
}
