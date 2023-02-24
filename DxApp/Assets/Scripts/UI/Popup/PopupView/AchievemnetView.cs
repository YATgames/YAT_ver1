using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Item;
using Assets.Scripts.UI.Util;
using Assets.Scripts.Util;
using DXApp_AppData.Table;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class AchievemnetView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        public ItemManager ItemManager { get; set; }
        public SoundManager SoundManager { get; set; }


        [SerializeField] private ReuseComponent _reuseComponenet;

        [SerializeField] private Button _homeButton;

        private QuestInfo[] _datas;

        private OnEventTrigger<List<string>> _onClickSynergy = new OnEventTrigger<List<string>>();
        private OnEventTrigger<QuestInfo> _onClickReward = new OnEventTrigger<QuestInfo>();

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            #region :::::: ClickEvent
            _onClickSynergy.AsObservable().Subscribe(data =>
            {
                SoundManager.Play("Button_Touch");
                FlowManager.AddSubPopup(PopupStyle.AchieveSynergyInfo, data);
            }).AddTo(gameObject);

            _onClickReward.AsObservable().Subscribe(data =>
            {
                SoundManager.Play("Tagging_Touch");
                FlowManager.AddSubPopup(PopupStyle.AchieveReward, data);
            }).AddTo(gameObject);

            _homeButton.OnClickAsObservable().Subscribe(_ =>
            {
                SoundManager.Play("Button_Click");
                FlowManager.Change(PopupStyle.Lobby);
            }).AddTo(gameObject);
            #endregion

            _reuseComponenet.UpdateItem += UpdateItem;
            _reuseComponenet.FirstLoadComplete.AddListener(() =>
            {
                for (int i = 0; i < _reuseComponenet.AutoCreateCount; i++)
                {
                    var obj = _reuseComponenet.GetContext<ItemAchievement_Line>(i);
                    var item = obj.ItemAchieveBlocks;
                    for (int j = 0; j < item.Length; j++)
                    {
                        item[j].OnClickSynergy = _onClickSynergy;
                    }

                    obj.ItemRewardBlock.OnClickReward = _onClickReward;
                }
            });
        }

        public void SetData(QuestInfo[] datas)
        {
            _datas = datas;
            _reuseComponenet.SetCount(datas.Length);
        }

        private void UpdateItem(int index, GameObject go)
        {
            var obj = go.GetComponent<ItemAchievement_Line>();
            var data = _datas[index];

            var spritesIndex = index % obj.QuestBgSpritesLengh;

            obj.SetData(data, spritesIndex, PlayerViewModel.Inventory, PlayerViewModel.Player);
        }
    }
}
