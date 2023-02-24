using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DXApp_AppData.Table;

namespace Assets.Scripts.UI.Item
{
    public class ItemAchievement_Block : MonoBehaviour
    {
        [SerializeField] private Button _clickButton;
        [SerializeField] private Image[] _images;
        [SerializeField] private Image _screenBg;

        [SerializeField] private Image _cardBgImage;
        [SerializeField] private Image _cardFrameImage;
        [SerializeField] private Sprite[] _cardBgSprits;
        [SerializeField] private Sprite[] _cardFrameSprits;
        public bool HasSynergy { get; set; }

        public OnEventTrigger<List<string>> OnClickSynergy;
        public OnEventTrigger<QuestInfo> OnClickReward;

        private List<string> _synergyPartsData;
        private QuestInfo _rewardData;

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _clickButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (_synergyPartsData != null)
                {
                    OnClickSynergy.Invoke(_synergyPartsData);
                }
                else
                {
                    if (HasSynergy)
                        OnClickReward.Invoke(_rewardData);
                    else
                        SoundManager.Instance.Play("ButtonFail_SFX");
                }
            }).AddTo(gameObject);
        }

        #region ::::::SetData(Sysnergy)
        public void SetData(List<string> data, int spriteIndex)
        {
            _cardBgImage.sprite = _cardBgSprits[spriteIndex];
            _cardFrameImage.sprite = _cardFrameSprits[spriteIndex];
            _synergyPartsData = data;

            for (int i = 0; i < _images.Length; i++)
            {
                for (int j = 0; j < data.Count; j++)
                {
                    var type = ItemManager.GetParts(data[j]).CustomData.PartsType;
                    if (i == (int)type)
                    {
                        _images[i].gameObject.SetActive(true);
                        _images[i].sprite = ResourcesManager.GetImages(data[j]);
                        break;
                    }
                    else
                    {
                        _images[i].gameObject.SetActive(false);
                    }
                }
            }

            if (HasSynergy) _screenBg.color = new Color(0, 0, 0, 0);
            else _screenBg.color = new Color(0, 0, 0, ((float)125 / (float)255));
        }
        #endregion

        #region ::::::SetData(Reward)
        public void SetData(QuestInfo data, int spriteIndex)
        {
            _cardBgImage.sprite = _cardBgSprits[spriteIndex];
            _cardFrameImage.sprite = _cardFrameSprits[spriteIndex];
            _rewardData = data;

            if (_rewardData.Reward.Substring(0, 1) == "6") // Å×¸¶
            {
                var thema = ItemManager.GetTheme(_rewardData.Reward);

                for (int i = 0; i < _images.Length; i++)
                {
                    _images[i].sprite = ResourcesManager.GetTheme(thema.ID);

                    Vector2 rewardAnchoredPosition = new Vector2(thema.CustomData.PosX * 0.6f, thema.CustomData.PosY * 0.6f);
                    _images[i].GetComponent<RectTransform>().anchoredPosition = rewardAnchoredPosition;

                    Vector2 rewardSizeDelta = new Vector2(thema.CustomData.Width * 0.6f, thema.CustomData.Height * 0.6f);
                    _images[i].GetComponent<RectTransform>().sizeDelta = rewardSizeDelta;
                }
            }
            else
            {
                var reward = ItemManager.GetParts(data.Reward);
                for (int i = 0; i < _images.Length; i++)
                {
                    _images[i].sprite = ResourcesManager.GetImages(reward.ID);

                    Vector2 rewardAnchoredPosition = new Vector2(reward.CustomData.PosX * 0.6f, reward.CustomData.PosY * 0.6f);
                    _images[i].GetComponent<RectTransform>().anchoredPosition = rewardAnchoredPosition;

                    Vector2 rewardSizeDelta = new Vector2(reward.CustomData.Width * 0.6f, reward.CustomData.Height * 0.6f);
                    _images[i].GetComponent<RectTransform>().sizeDelta = rewardSizeDelta;
                }
            }

            if (HasSynergy) _screenBg.color = new Color(0, 0, 0, 0);
            else _screenBg.color = new Color(0, 0, 0, ((float)125 / (float)255));
        }
        #endregion
    }
}
