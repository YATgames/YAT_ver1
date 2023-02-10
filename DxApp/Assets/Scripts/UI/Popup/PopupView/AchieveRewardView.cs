using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Common;
using Assets.Scripts.UI.Popup.Sup;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class AchieveRewardView : MonoBehaviour
    {
        public ItemManager ItemManager { get; set; }
        public ConnectionManager ConnectionManager { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }

        public UIPopupAchieveReward UIPopupAchieveReward { get; set; }

        [SerializeField] private Image _rewardImage;
        [SerializeField] private Button _closeButton;

        [SerializeField] private Text _rewardName;

        [SerializeField] private Button _rewardButton;

        private QuestInfo _questInfo;
        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _closeButton.OnClickAsObservable().Subscribe(_ => UIPopupAchieveReward.Hide()).AddTo(gameObject);

            _rewardButton.OnClickAsObservable().Subscribe(_ =>
            {
                //리워드 아이템 지급
                ConnectionManager.ClearedQuest(_questInfo);
                SystemLoading.Show(SystemLoading.LoadingSize.Big, this);
            }).AddTo(gameObject);

            PlayerViewModel.ServerRespones.AsObservable().Subscribe(res =>
            {
                UIPopupAchieveReward.Hide();
                SystemLoading.Hide(this);
            }).AddTo(gameObject);
        }
        public void SetData(QuestInfo questInfo)
        {
            _questInfo = questInfo;
            var rewardID = questInfo.Reward;

            if(rewardID.Substring(0,1) == "6") // 테마
            {
                var thema = ItemManager.GetTheme(rewardID);

                var rect = _rewardImage.GetComponent<RectTransform>();

                _rewardImage.sprite = ResourcesManager.GetTheme(thema.ID);
                _rewardName.text = thema.Name.FromStringTable();

                Vector2 anchoredPosition = new Vector2(thema.CustomData.PosX, thema.CustomData.PosY);
                Vector2 sizeDelta = new Vector2(thema.CustomData.Width, thema.CustomData.Height);

                rect.anchoredPosition = anchoredPosition * 0.9f;
                rect.sizeDelta = sizeDelta *0.9f;
            }
            else
            {
                var Parts = ItemManager.GetParts(rewardID);

                if (Parts == null) return;

                var rect = _rewardImage.GetComponent<RectTransform>();
                _rewardImage.sprite = ResourcesManager.GetImages(Parts.ID);
                _rewardName.text = Parts.Name.FromStringTable();
                Vector2 anchoredPosition = new Vector2(Parts.CustomData.PosX, Parts.CustomData.PosY);
                Vector2 sizeDelta = new Vector2(Parts.CustomData.Width, Parts.CustomData.Height);

                rect.anchoredPosition = anchoredPosition*0.9f;
                rect.sizeDelta = sizeDelta*0.9f;
            }
        }
    }
}
