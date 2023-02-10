using Assets.Scripts.Managers;
using DXApp_AppData.Model;
using DXApp_AppData.Table;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using DXApp_AppData.Item;
using Assets.Scripts.Common.Models;

namespace Assets.Scripts.UI.Item
{
    public class ItemAchievement_Line : MonoBehaviour
    {
        [SerializeField] private Text _synergyNameText;
        [SerializeField] private Image _completeImg;
        [SerializeField] private Image _questBg;
        [SerializeField] private Sprite[] _questBgSprites;

        [SerializeField] private ItemAchievement_Block[] _itemAchieve_Blocks;
        [SerializeField] private ItemAchievement_Block _itemReward;

        public int QuestBgSpritesLengh { get { return _questBgSprites.Length; } }
        public ItemAchievement_Block[] ItemAchieveBlocks { get { return _itemAchieve_Blocks; } }
        public ItemAchievement_Block ItemRewardBlock { get { return _itemReward; } }

        public void SetData(QuestInfo data,int spriteIndex, InventoryModel inventory, PlayerModel player)
        {
            // ��������Ʈ ���� ����
            _questBg.sprite = _questBgSprites[spriteIndex];

            // Ŭ���� üũ
            if (null != player.ClearedQuestID.FirstOrDefault(v => v == data.ID.ToString()))
                _completeImg.gameObject.SetActive(true);
            else
                _completeImg.gameObject.SetActive(false);

            #region ::::::SetData(Character)
            bool[] _hasSynergys = new bool[data.RequiresParts.Count];

            //  data�� ������ŭ�� ItemBlock�� ����
            for (int i = 0; i < ItemAchieveBlocks.Length; i++)
            {
                if (i < data.RequiresParts.Count) ItemAchieveBlocks[i].gameObject.SetActive(true);
                else ItemAchieveBlocks[i].gameObject.SetActive(false);
            }

            _synergyNameText.text = data.Name;

            for (int i = 0; i < data.RequiresParts.Count; i++)
            {
                // ������ �ǱԾ�� �Ȱ��� �ǱԾ �ִ��� ������ ���ؼ� ã�´�
                List<OriginFigure> originFigureContainer = new List<OriginFigure>();
                List<PlayfabItemInstance> originFigureData = new List<PlayfabItemInstance>();
                _itemAchieve_Blocks[i].HasSynergy = false;

                if (inventory.OriginFigures != null)
                {
                    for (int j = 0; j < inventory.OriginFigures.Count; j++)
                    {
                        PlayfabItemInstance item = inventory.OriginFigures[j];
                        originFigureData.Add(item);
                        var figure = ItemManager.Instance.Figures.FirstOrDefault(v => v.ID == item.ID);
                        if (figure != null) originFigureContainer.Add(figure);
                    }

                    for (int j = 0; j < data.RequiresParts[i].Count; j++)
                    {
                        originFigureContainer = originFigureContainer.Where(v => v.Items.Contains(data.RequiresParts[i][j])).ToList();
                    }

                    if (originFigureContainer.Count > 0)
                    {
                        _hasSynergys[i] = true;
                        _itemAchieve_Blocks[i].HasSynergy = true;
                        _itemAchieve_Blocks[i].SetData(originFigureContainer[0].Items, spriteIndex);
                        continue;
                    }
                }

                // Ŀ���� �ǱԾ�� �Ȱ��� �ǱԾ �ִ��� ������ ���ؼ� ã�´�
                List<CustomFigureInstance> customFigureContainer = new List<CustomFigureInstance>();
                customFigureContainer = inventory.CustomFigures;
                for (int j = 0; j < data.RequiresParts[i].Count; j++)
                {
                    customFigureContainer = customFigureContainer.Where(v => v.CustomData.Parts.Contains(data.RequiresParts[i][j])).ToList();
                }
                if (customFigureContainer.Count > 0)
                {
                    _hasSynergys[i] = true;
                    _itemAchieve_Blocks[i].HasSynergy = true;
                    _itemAchieve_Blocks[i].SetData(customFigureContainer[0].CustomData.Parts, spriteIndex);
                    continue;
                }

                // ������ �ǱԾ ������ �� ������
                _itemAchieve_Blocks[i].SetData(data.RequiresParts[i], spriteIndex);
            }

            #endregion

            #region ::::::SetData(Reward)
            // ������ ��� �ó��� ĳ���Ͱ� �����ϴ��� �ľ��� Reward�� HasSyenrgy�� Ų��.
            _itemReward.HasSynergy = true;
            for (int i = 0; i < _hasSynergys.Length; i++)
            {
                if (_hasSynergys[i] == false)
                {
                    _itemReward.HasSynergy = false;
                    break;
                }
            }
            _itemReward.SetData(data, spriteIndex);
            #endregion
        }
    }
}
