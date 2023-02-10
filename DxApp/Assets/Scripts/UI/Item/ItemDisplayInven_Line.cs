using Assets.Scripts.Util;
using DXApp_AppData.Item;
using UnityEngine;

namespace Assets.Scripts.UI.Item
{
	public class ItemDisplayInven_Line : MonoBehaviour
	{
		public int Count { get { return _itemBlocks.Length; } }

		[SerializeField] ItemDisplayInven_Block[] _itemBlocks;
		public OnEventTrigger<PlayfabItemInstance> OnClick { get; set; }

		public void Initialize()
		{
			for (int i = 0; i < _itemBlocks.Length; i++)
			{
				_itemBlocks[i].OnClick = OnClick;
			}
		}

		public void SetEmpty(int i)
		{
			_itemBlocks[i].SetEmpty();
		}

		public void SetData(int i, OriginFigureInstance item, bool isSelected)
		{
			_itemBlocks[i].SetData(item, isSelected);
		}

		public void SetData(int i, CustomFigureInstance item, bool isSelected)
		{
			_itemBlocks[i].SetData(item, isSelected);
		}
	}
}
