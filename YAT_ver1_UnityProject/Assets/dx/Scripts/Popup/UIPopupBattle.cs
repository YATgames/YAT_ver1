using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Manager;
using Assets.Scripts.Common.DI;
using System.Runtime.InteropServices;

// BattleScene���� �̵��Ǵ� Scnee���� ����ϴ� UI �뵵
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
                Debug.Log("BattlePopup�� Ȱ��ȭ �Ǿ�����" +
                    "data�� ����־ ��ȯ ��");
                return;
            }
        }
        public override void Hide()
        {
            base.Hide();
        }
    }
}