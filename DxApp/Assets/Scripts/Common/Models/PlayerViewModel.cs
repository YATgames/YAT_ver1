using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Common.DI;
using DXApp_AppData.Model;
using DXApp_AppData.Item;
using DXApp_AppData.Server;
using DXApp_AppData.Enum;
using DXApp_AppData.Table;

namespace Assets.Scripts.Common.Models
{
	public class PlayerViewModel : UnitySingleton<PlayerViewModel>
	{
		public InventoryModel Inventory { get; set; }
		public AccountModel Account { get; set; }
		public PlayerModel Player { get; set; }

        public OnEventTrigger UpdateItem = new OnEventTrigger();
		public OnEventTrigger ServerRespones = new OnEventTrigger();

        public OnEventTrigger<bool> OnPartArchive = new OnEventTrigger<bool>();
        public OnEventTrigger<bool> OnFigureArchive = new OnEventTrigger<bool>();

        #region ::::::PlayerDatas

        public PlayfabItemInstance FigureArchive { get; set; }
        public FigureTypeTable FigureTypeTable { get; set; }
        public List<PartsInstance> PartsArchive { get; set; } = new List<PartsInstance> { };

        #endregion

        private void CombineViewUpdate()
        {
            bool buttonActive = false;
            var ArchivebodyCheck = PartsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Body);
            var ArchiveheadCheck = PartsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Head);
            if (ArchivebodyCheck != null && ArchiveheadCheck != null)
            {
                buttonActive = true;
            }

            OnPartArchive.Invoke(buttonActive);
            UpdateItem.Invoke();
        }

        public override void Initialize()
        {
            DependuncyInjection.Inject(this);
            base.Initialize();
        }

        public void Reset()
        {
            FigureTypeTable = null;
            FigureArchive = null;
            PartsArchive.Clear();

            UpdateItem.Invoke();
        }

        #region ::::::SetDatas

        public void SetData(FigureTypeTable data)
        {
            if (data == FigureTypeTable) FigureTypeTable = null;
            else FigureTypeTable = data;

            UpdateItem.Invoke();
        }
        public void SetData(PartsInstance data)
        {
            if (PartsArchive.Contains(data))
            {
                PartsArchive.Remove(data);

                CombineViewUpdate();

                return;
            }

            data.CustomData = ItemManager.Instance.PartsList.FirstOrDefault(v => v.ID == data.ID).CustomData;

            switch (data.CustomData.PartsType)
            {
                case PartsType.Body:
                    var bodyPart = PartsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Body);
                    if (bodyPart != null) PartsArchive.Remove(bodyPart);
                    PartsArchive.Add(data);
                    break;

                case PartsType.Head:
                    var headPart = PartsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Head);
                    if (headPart != null) PartsArchive.Remove(headPart);
                    PartsArchive.Add(data);
                    break;

                case PartsType.Postdeco:
                case PartsType.Predeco:
                    var decoPart = PartsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Postdeco || v.CustomData.PartsType == PartsType.Predeco);
                    if (decoPart != null) PartsArchive.Remove(decoPart);
                    PartsArchive.Add(data);
                    break;

                default:
                    Debug.LogError("PartInstance Error! PartsType is Wrong! data Instance = " + data.InstanceID + " PartsType =" + data.CustomData.PartsType);
                    break;
            }

            CombineViewUpdate();
        }

        public void SetData(PlayfabItemInstance data)
        {
            bool BreakButtonActive = false;
            FigureArchive = data;

            if (data is CustomFigureInstance) BreakButtonActive = true;
            else BreakButtonActive = false;

            OnFigureArchive.Invoke(BreakButtonActive);
            UpdateItem.Invoke();
        }

        public void SetDataCase(PlayfabItemInstance data,string caseinfo)
        {
           // ItemManager item;
           //오리진 피규어면 itemmanager.인스턴스가 있음

        }

        #endregion

    }
}

