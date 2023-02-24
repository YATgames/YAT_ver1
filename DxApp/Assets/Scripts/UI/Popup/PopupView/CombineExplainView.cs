using Assets.Scripts.Common;
using Assets.Scripts.Common.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;
using System;
using UniRx;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class CombineExplainView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }

        [SerializeField] private Button _gotoNFCButton;

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            if(_gotoNFCButton != null) _gotoNFCButton.OnClickAsObservable().Subscribe(_ => FlowManager.Change(PopupStyle.NFC)).AddTo(gameObject);
        }
    }
}
