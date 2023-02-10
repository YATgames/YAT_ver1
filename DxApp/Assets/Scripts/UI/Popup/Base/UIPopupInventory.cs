using Assets.Scripts.Common.DI;
using Assets.Scripts.UI.Popup.PopupView;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using UniRx;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupInventory : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourceManager;
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;
        [DependuncyInjection(typeof(ConnectionManager))]
        private ConnectionManager _connectionManager;

        [SerializeField] private InventoryView _inventoryView;

        public override void Initialize()
        {
            DependuncyInjection.Inject(this);
            base.Initialize();

            _inventoryView.FlowManager = _flowManager;
            _inventoryView.ResourcesManager = _resourceManager;
            _inventoryView.PlayerViewModel = _playerViewModel;
            _inventoryView.ConnectionManager = _connectionManager;

            _playerViewModel.UpdateItem.
                AsObservable().
                Subscribe(_ => _inventoryView.SetData()).
                AddTo(gameObject);
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
            if (data.Length != 0 && data[0] is bool)
            {
                // 합체 성공후 피규어 보러오기
                string FigureTitle = data[1].ToString();

                for (int i = 0; i < _playerViewModel.Inventory.CustomFigures.Count; i++)
                {
                    if (_playerViewModel.Inventory.CustomFigures[i].CustomData.Name == FigureTitle)
                    {
                        _playerViewModel.FigureArchive = _playerViewModel.Inventory.CustomFigures[i];
                        break;
                    }
                }
            }
            else
            {
                FindFavoriteFigure();
            }
            _inventoryView.Initialize();
        }

        public override void Hide()
        {
            base.Hide();
            _playerViewModel.Reset();
        }

        private void FindFavoriteFigure()
        {
            bool findFavoriteFigure = false;
            for (int i = 0; i < _playerViewModel.Inventory.OriginFigures.Count; i++)
            {
                if (_playerViewModel.Player.FavoriteInstanceID == _playerViewModel.Inventory.OriginFigures[i].InstanceID)
                {
                    _playerViewModel.FigureArchive = _playerViewModel.Inventory.OriginFigures[i];
                    findFavoriteFigure = true;
                    break;
                }
            }

            if (!findFavoriteFigure)
            {
                for (int i = 0; i < _playerViewModel.Inventory.CustomFigures.Count; i++)
                {
                    if (_playerViewModel.Player.FavoriteInstanceID == _playerViewModel.Inventory.CustomFigures[i].InstanceID)
                    {
                        _playerViewModel.FigureArchive = _playerViewModel.Inventory.CustomFigures[i];
                        break;
                    }
                }
            }
        }
    }
}
