using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI
{
    public class PopupManager : UnitySingleton<PopupManager> 
    {
        // �̱������� ������Ű�� ��ü. ��ɵ� ���⿡ ������.


        // = Constant
        private const string PopupPrefix = "Prefs/Popup/UIPopup";

        // = Field
        public readonly List<PopupBase> PopupList = new List<PopupBase>();

        // ī�޶� �Ŵ��� ���� ��������

        public override void Initialize()
        {
            base.Initialize();
            //DependuncyInjection.Inject(this);
        }
        public override void UnInitialize()
        {
            //base.UnInitialize();
            foreach(var popup in PopupList)
            {
                Destroy(popup.gameObject);
            }
            PopupList.Clear();
        }

        
    }
}