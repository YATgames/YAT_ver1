using UnityEngine;
using Assets.Scripts.UI.Util;
using System;
using System.Collections.Generic;
using UniRx;
using Assets.Scripts.UI.Item;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using Assets.Scripts.Common.Models;
using System.Linq;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class CaseInvenView : MonoBehaviour
	{
		public PlayerViewModel PlayerViewModel { get; set; }

        [SerializeField] private ReuseComponent _reuseComponent;
		[SerializeField] private ItemCase_Line _originLine;

		private List<Theme> _datas = new List<Theme>();

		public OnEventTrigger<string> OnClick { get; set; }

		private int _caseNum;

		public void Initialize()
		{
			AddEvent();
			_reuseComponent.UpdateItem += UpdateItem;
		}

		public void SetData(List<Theme> datas, int caseNum)
		{
			var cellCount = _originLine.Count;
			var count = (datas.Count / cellCount) + (datas.Count % cellCount == 0 ? 0 : 1);
			_reuseComponent.SetCount(Mathf.Max(2, count));
			_caseNum = caseNum;
            _datas = datas;
		}

		private void AddEvent()
		{
			_reuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
			{
				for (int i = 0; i < _reuseComponent.AutoCreateCount; i++)
				{
					var item = _reuseComponent.GetContext<ItemCase_Line>(i);
					item.OnClick = OnClick;
					item.Initialize();
				}
			}).AddTo(gameObject);

		}

		private void UpdateItem(int index, GameObject go)
		{
			var item = go.GetComponent<ItemCase_Line>();
			var lineCount = item.Count;
			var initValue = lineCount * index;

			for (int i = initValue; i < initValue + lineCount; i++)
			{
				var temaIndex = i - initValue;

				if (_datas.Count <= i)
					item.SetData(temaIndex , null, false);
				else
				{
                    var caseInfo = PlayerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == _caseNum);

					bool isSelected = caseInfo.ThemeID.Equals(_datas[i].ID);

                    var data = _datas[i];
					item.SetData(temaIndex, data, isSelected);
				}
			}
		}
	}

}