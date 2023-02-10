using Assets.Scripts.Util;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Item
{
    public class ItemProperty_Line : MonoBehaviour
    {
        [SerializeField] private ItemProperty_Block[] _itemProperty_Blocks;
        public ItemProperty_Block[] ItemPropertyBlocks { get { return _itemProperty_Blocks; } }
        public OnEventTrigger<FigureTypeTable> OnClick { get; set; }

        public void Initialize()
        {
            for (int i = 0; i < _itemProperty_Blocks.Length; i++)
            {
                _itemProperty_Blocks[i].OnClick = OnClick;
            }
        }
        public void SetEmpty(int i)
        {
            _itemProperty_Blocks[i].SetEmpty();
        }

        public void SetData(int i, FigureTypeTable item, bool isSelect)
        {
            _itemProperty_Blocks[i].SetData(item, isSelect);
        }
    }
}
