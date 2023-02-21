using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Manager;
using Assets.Scripts.Common.DI;
using System.Runtime.InteropServices;

// BattleScene에서 이동되는 Scnee에서 사용하는 UI 용도
namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupBattle : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;

        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);
        }
        public override void UnInitialzie()
        {
            base.UnInitialzie();
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
            if(data.Length == 0 || data[0] is int == false)
            {
                //Hide();
                Debug.Log("BattlePopup이 활성화 되었지만" +
                    "data가 비어있어서 반환 함");
                return;
            }
        }
        public override void Hide()
        {
            base.Hide();
        }
    }
}