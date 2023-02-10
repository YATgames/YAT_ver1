using Assets.Scripts.Common;
using Assets.Scripts.Common.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using DXApp_AppData.Util;
using System;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class InventoryExplainView : MonoBehaviour
    {
        public ConfigManager ConfigManager { get; set; }
        public ItemManager ItemManager { get; set; }

        [SerializeField] private Text _typeText;
        [SerializeField] private Text _powerText;
        [SerializeField] private Text _headText;
        [SerializeField] private Text _bodyText;
        [SerializeField] private Text _decoText;
        [SerializeField] private Text _createDayText;

        private const string NONE_PROPERTY = "¹«¼Ó¼º";
        private void ResetText()
        {
            _typeText.text = "";
            _powerText.text = "";
            _headText.text = "";
            _bodyText.text = "";
            _decoText.text = "";
            _createDayText.text = "";
        }

        public void SetData(PlayfabItemInstance data)
        {
            int power = 0;
            ResetText();

            if (data is OriginFigureInstance)
            {
                var items = ItemManager.Figures.FirstOrDefault(v => v.ID == data.ID).Items;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = ItemManager.PartsList.FirstOrDefault(v => v.ID == items[i]);
                    if (item.CustomData.PartsType == PartsType.Head) _headText.text = item.Name.FromStringTable();
                    else if (item.CustomData.PartsType == PartsType.Body) _bodyText.text = item.Name.FromStringTable();
                    else if (item.CustomData.PartsType == PartsType.Postdeco || item.CustomData.PartsType == PartsType.Predeco) _decoText.text = item.Name.FromStringTable();

                    power += item.CustomData.Power;
                }

                _createDayText.text = data.PurchaseDate.ToString();
                _typeText.text = NONE_PROPERTY;
                _powerText.text = power.ToString();
            }

            if(data is CustomFigureInstance)
            {
                var customFigureInstance = data as CustomFigureInstance;

                var parts = customFigureInstance.CustomData.Parts;

                for (int i = 0; i < parts.Count; i++)
                {
                    var item = ItemManager.GetParts(parts[i]);

                    if (item.CustomData.PartsType == PartsType.Head) _headText.text = item.Name.FromStringTable();
                    else if (item.CustomData.PartsType == PartsType.Body) _bodyText.text = item.Name.FromStringTable();
                    else if (item.CustomData.PartsType == PartsType.Postdeco || item.CustomData.PartsType == PartsType.Predeco) _decoText.text = item.Name.FromStringTable();

                    power += item.CustomData.Power;
                }


                if (data.PurchaseDate.HasValue)
                {
                    _createDayText.text = data.PurchaseDate.Value.Tokr().ToString();
                }
                else _createDayText.text = String.Empty;
                _typeText.text =  ConfigManager.GetFigureTypeTable(customFigureInstance.CustomData.FigureType).Name;
                _powerText.text = power.ToString();
            }

            _powerText.text = "+ " + power;
        }
    }
}
