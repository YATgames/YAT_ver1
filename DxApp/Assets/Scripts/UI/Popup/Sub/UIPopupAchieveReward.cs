using Assets.Scripts.Common.DI;
using UnityEngine;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using DXApp_AppData.Table;

namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupAchieveReward : PopupSub
    {
        [DependuncyInjection(typeof(ItemManager))]
        private ItemManager _itemManager;
        [DependuncyInjection(typeof(ConnectionManager))]
        private ConnectionManager _connectionManager;
        [DependuncyInjection(typeof(PlayerViewModel))]
        private PlayerViewModel _playerViewModel;

        [SerializeField] private AchieveRewardView _achieveRewardView;


        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _achieveRewardView.ItemManager = _itemManager;
            _achieveRewardView.ConnectionManager = _connectionManager;
            _achieveRewardView.PlayerViewModel = _playerViewModel;

            _achieveRewardView.UIPopupAchieveReward = this;
        }

        
        public override void Show(params object[] data)
        {
            base.Show(data);

            QuestInfo questInfo = data[0] as QuestInfo;
            _achieveRewardView.SetData(questInfo);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
