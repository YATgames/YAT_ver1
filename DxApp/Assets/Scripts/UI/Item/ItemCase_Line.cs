using Assets.Scripts.Util;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using UnityEngine;

namespace Assets.Scripts.UI.Item
{
	public class ItemCase_Line : MonoBehaviour
	{
		public int Count { get { return _blocks.Length; } }
		[SerializeField] private ItemCase_Block[] _blocks;
		public OnEventTrigger<string> OnClick { get; set; }

		public void Initialize()
		{
			for (int i = 0; i < _blocks.Length; i++)
			{
				_blocks[i].OnClick = OnClick;
			}
		}

		public void SetData(int i , Theme theme, bool isSelect)
		{
			_blocks[i].SetActiveImage(theme != null);

			if(theme != null)
				_blocks[i].SetData(theme, isSelect);
		}
	}
}
