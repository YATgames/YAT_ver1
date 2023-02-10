using Assets.Scripts.Util;
using DXApp_AppData.Item;
using UnityEngine;

namespace Assets.Scripts.UI.Item
{
    public class ItemInventory_Line : MonoBehaviour
    {
        [SerializeField] private ItemInventory_Block[] _itemInventoryBlocks;
        public ItemInventory_Block[] ItemInventoryBlocks { get { return _itemInventoryBlocks; } }

        public OnEventTrigger<PlayfabItemInstance> OnClick { get; set; }

        public void Initialize()
        {
            for (int i = 0; i < ItemInventoryBlocks.Length; i++)
            {
                _itemInventoryBlocks[i].OnClick = OnClick; 
            }
        }
        public void SetEmpty(int i)
        {
            _itemInventoryBlocks[i].SetEmpty();
        }

        public void SetData(int i, OriginFigureInstance item, bool isSelect)
        {
            _itemInventoryBlocks[i].SetData(item, isSelect);
        }

        public void SetData(int i, CustomFigureInstance item, bool isSelect)
        {
            _itemInventoryBlocks[i].SetData(item, isSelect);
        }
    }
}
