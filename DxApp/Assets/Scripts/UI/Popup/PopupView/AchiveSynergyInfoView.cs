using Assets.Scripts.Managers;
using Assets.Scripts.UI.Item;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DXApp_AppData.Item;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class AchiveSynergyInfoView : MonoBehaviour
    {
        [SerializeField] private ItemAchieveSynergy_itemInfo[] _itemAchieveSynergy_ItemInfo;

        public void SetData(List<string> data)
        {
            List<Parts> PartsList = new List<Parts>();

            for (int i = 0; i < data.Count; i++)
            {
                var item = ItemManager.GetParts(data[i]);
                PartsList.Add(item);
            }

            for (int i = 0; i < _itemAchieveSynergy_ItemInfo.Length; i++)
            {
                var typeItem = PartsList.FirstOrDefault(v => (int)v.CustomData.PartsType == i);
                if (typeItem != null)
                {
                    var rect = _itemAchieveSynergy_ItemInfo[i].ItemImg.GetComponent<RectTransform>();
                    Vector2 anchoredPosition = new Vector2(typeItem.CustomData.PosX, typeItem.CustomData.PosY);
                    Vector2 sizeDelta = new Vector2(typeItem.CustomData.Width, typeItem.CustomData.Height);

                    _itemAchieveSynergy_ItemInfo[i].ItemImg.sprite = ResourcesManager.GetImages(typeItem.ID);
                    _itemAchieveSynergy_ItemInfo[i].ItemText.text = typeItem.Name.FromStringTable();
                    rect.anchoredPosition = anchoredPosition*0.4f;
                    rect.sizeDelta = sizeDelta*0.15f;
                }
                else
                {
                    _itemAchieveSynergy_ItemInfo[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
