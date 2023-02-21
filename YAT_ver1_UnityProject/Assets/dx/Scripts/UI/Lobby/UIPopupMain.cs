using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.UI.Popup.PopupView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupMain : PopupBase
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;        
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;
        [SerializeField] private MainView _mainView;

        public readonly OnEventTrigger<PopupBase> LoadCompletePopup = new OnEventTrigger<PopupBase>();

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _mainView.FlowManager = _flowManager;
            _mainView.ResourcesManager = _resourcesManager;
            _mainView.SoundManager = _soundManager;
            _mainView.SetData(); // view �� ������ ����
            // ConnectionLogin �� ���۵Ǹ� ���⼭ �α��� �˻�

            // Ŀ�ؼ� �Ŵ��� �˻�

            // BGM ����
            //SystemLoading.Hide(this);
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
        }
        public override void Hide()
        {
            base.Hide();
        }
    }
}