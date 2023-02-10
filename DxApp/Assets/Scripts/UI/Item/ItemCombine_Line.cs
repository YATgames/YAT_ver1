using Assets.Scripts.Util;
using DXApp_AppData.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Item
{
    public class ItemCombine_Line : MonoBehaviour
    {
        [SerializeField] private ItemCombine_Block[] _itemCombine_Blocks;
        public ItemCombine_Block[] ItemCombineBlocks { get { return _itemCombine_Blocks; } }

        public OnEventTrigger<PartsInstance> OnClick { get; set; }

        public void Initialize()
        {
            for (int i = 0; i < _itemCombine_Blocks.Length; i++)
            {
                _itemCombine_Blocks[i].OnClick = OnClick;
            }
        }
        public void SetEmpty(int i)
        {
            _itemCombine_Blocks[i].SetEmpty();
        }

        public void SetData(int i, PartsInstance item, bool isSelect)
        {
            _itemCombine_Blocks[i].SetData(item, isSelect);
        }
    }
}
