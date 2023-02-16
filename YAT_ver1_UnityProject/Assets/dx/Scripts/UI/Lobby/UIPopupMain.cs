using Assets.Scripts.Common.DI;
using Assets.Scripts.Manager;
using Assets.Scripts.UI.Popup.PopupView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


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


        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _mainView.FlowManager = _flowManager;
            _mainView.ResourcesManager = _resourcesManager;
            _mainView.SoundManager = _soundManager;
            _mainView.SetData(); // view 의 데이터 세팅
            // ConnectionLogin 이 제작되면 여기서 로그인 검사
            // 커넥션 매니저 검사

            // BGM 동작
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