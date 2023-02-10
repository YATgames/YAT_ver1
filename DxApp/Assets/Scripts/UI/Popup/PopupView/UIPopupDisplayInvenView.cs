using Assets.Scripts.Common.Models;
using Assets.Scripts.UI.Item;
using Assets.Scripts.UI.Util;
using Assets.Scripts.Util;
using DXApp_AppData.Item;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class UIPopupDisplayInvenView : MonoBehaviour
	{
		public PlayerViewModel PlayerViewModel { get; set; }

		[SerializeField] private ItemDisplayInven_Line _originItem;
		[SerializeField] private ReuseComponent _originReuseComponent;
		[SerializeField] private ReuseComponent _customReuseComponent;

		public OnEventTrigger<PlayfabItemInstance> OnClick { get; set; }

		private List<CustomFigureInstance> CustomFigures { get; set; }
		private List<OriginFigureInstance> OriginFigures { get; set; }

		private int _caseNumber;

        public void Initialize()
		{
            AddEvent();

			CustomFigures = PlayerViewModel.Inventory.CustomFigures;
			OriginFigures = PlayerViewModel.Inventory.OriginFigures;

			_originReuseComponent.UpdateItem += Origin_UpdateItem;
			_customReuseComponent.UpdateItem += Custom_UpdateItem;

			var cellCount = _originItem.Count;
			var originCount = (OriginFigures.Count / cellCount) + (OriginFigures.Count % cellCount == 0 ? 0 : 1);
			var customCount = (CustomFigures.Count / cellCount) + (CustomFigures.Count % cellCount == 0 ? 0 : 1);

			_originReuseComponent.SetCount(Mathf.Max(2 , originCount));
			_customReuseComponent.SetCount(Mathf.Max(2 , customCount));
		}

		private void AddEvent()
		{
			_originReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
			{
				for (int i = 0; i < _originReuseComponent.AutoCreateCount; i++)
				{
					var item = _originReuseComponent.GetContext<ItemDisplayInven_Line>(i);
					item.OnClick = OnClick;
					item.Initialize();
				}
			}).AddTo(gameObject);

			_customReuseComponent.FirstLoadComplete.AsObservable().Subscribe(_ =>
			{
				for (int i = 0; i < _customReuseComponent.AutoCreateCount; i++)
				{
					var item = _customReuseComponent.GetContext<ItemDisplayInven_Line>(i);
					item.OnClick = OnClick;
					item.Initialize();
				}
			}).AddTo(gameObject);
		}

		private void Custom_UpdateItem(int index, GameObject go)
		{
			var item = go.GetComponent<ItemDisplayInven_Line>();
			var lineCount = item.Count;
			var initValue = lineCount * index;
			var datas = CustomFigures;

			for (int i = initValue; i < initValue + lineCount; i++)
			{
				var itemIndex = i - initValue;

				if (datas.Count <= i)
					item.SetEmpty(itemIndex);
				else
				{
                    var caseInfo = PlayerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == _caseNumber);

					bool isSelected = caseInfo.FigureInstanceID == datas[i].InstanceID;

                    var data = datas[i];
					item.SetData(itemIndex, data, isSelected);
				}
			}
		}
		private void Origin_UpdateItem(int index, GameObject go)
		{
			var item = go.GetComponent<ItemDisplayInven_Line>();
			var lineCount = item.Count;
			var initValue = lineCount * index;
			var datas = OriginFigures;

			for (int i = initValue; i < initValue + lineCount; i++)
			{
				var itemIndex = i - initValue;

				if (datas.Count <= i)
					item.SetEmpty(itemIndex);
				else
				{
                    var caseInfo = PlayerViewModel.Player.CaseList.FirstOrDefault(v => v.Number == _caseNumber);

                    bool isSelected =  (caseInfo.FigureInstanceID == datas[i].InstanceID);

                    var data = datas[i];
					item.SetData(itemIndex, data, isSelected);
				}
			}
		}

		public void SetData(int caseNumber)
		{
			_caseNumber = caseNumber;
        }
	}
}
