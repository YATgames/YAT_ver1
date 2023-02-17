using Assets.Scripts.Common.DI;
using Assets.Scripts.Common.Models;
using Assets.Scripts.UI.Item;
using Assets.Scripts.Util;
using DXApp_AppData.Table;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

using Assets.Scripts.Managers; // ResourcesManager 가져오기 위해서 추가함
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.PopupView
{
	public class LobbyView : MonoBehaviour
	{
		public PlayerViewModel PlayerViewModel { get; set; }
		public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }

        [SerializeField] private ItemDisplay[] _itemDisplays;

		[SerializeField] private Button _explainButton;

        [SerializeField] private GameObject _sinbiOBJ;
        [SerializeField] private GameObject _geumbiOBJ;

        private OnEventTrigger<int> _onClick = new OnEventTrigger<int>();
		private List<CaseInfo> _datas;

		private void Start()
		{
            for (int i = 0; i < _itemDisplays.Length; i++)
			{
				_itemDisplays[i].OnClick = _onClick;
			}

			var sinbi = PlayerViewModel.Inventory.Doggabis.FirstOrDefault(v => v.ID == "70001");
            var geumbi = PlayerViewModel.Inventory.Doggabis.FirstOrDefault(v => v.ID == "70002");

			if (sinbi != null) _sinbiOBJ.SetActive(true);
			else _sinbiOBJ.SetActive(false);

            if (geumbi != null) _geumbiOBJ.SetActive(true);
			else _geumbiOBJ.SetActive(false);

            AddEvent();
            DependuncyInjection.Inject(this);
        }

        private void AddEvent()
		{
			_onClick.AsObservable().Subscribe(caseNumber =>
			{
				SoundManager.Instance.Play("Button_Click");
				FlowManager.Change(PopupStyle.Custom, caseNumber);
			}).AddTo(gameObject);

			_explainButton.OnClickAsObservable().Subscribe(v => FlowManager.AddSubPopup(PopupStyle.LobbyExplain)).AddTo(gameObject);

        }

		public void SetData(List<CaseInfo> datas)
		{
			for (int i = 0; i < _itemDisplays.Length; i++)
			{

				var itemDisplay = _itemDisplays[i];
				var data = datas.FirstOrDefault(v => v.Number == itemDisplay.Number);
				if(data == null)
				{
					itemDisplay.gameObject.SetActive(false);
					LogManager.Error("LobbyView ::: Case Number {0} Not Found", itemDisplay.Number);
					continue;
				}

                itemDisplay.SetCase(data);
				if (string.IsNullOrEmpty(data.FigureInstanceID))
				{
					itemDisplay.SetEmpty();
				}
				else
				{
					itemDisplay.loadResource(ResourcesManager); // 피규어 정보가 있을때만 모델링 세팅하기
					var customFigure = PlayerViewModel.Inventory.CustomFigures.FirstOrDefault(v => v.InstanceID == data.FigureInstanceID);
					var originFigure = PlayerViewModel.Inventory.OriginFigures.FirstOrDefault(v => v.InstanceID == data.FigureInstanceID);

					if (customFigure != null)
					{
                        itemDisplay.SetData(customFigure);
                    }
                    else if (originFigure != null)
					{
                        itemDisplay.SetData(originFigure);
                    }
                    else
                        itemDisplay.SetEmpty();
                }

			}
			_datas = datas;
		}
	}
}
