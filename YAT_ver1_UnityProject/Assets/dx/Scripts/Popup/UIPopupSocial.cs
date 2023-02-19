using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Assets.Scripts.Manager;
using Assets.Scripts.Common.DI;
using Assets.Scripts.UI.Popup.PopupView;

namespace Assets.Scripts.UI.Popup.Sub
{
    public class UIPopupSocial : PopupSub
    {
        // ����ϰԵ� Manager DependencyInjeciton���� �޾ƿ���

        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
        [DependuncyInjection(typeof(GameManager))]
        private GameManager _gameManager;

        [SerializeField] private Button _closeButton; // subPopup ������ư

        // view�� ���߿� ������
        [SerializeField] private SocialView _socialView;
        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            _closeButton.OnClickAsObservable()
                .Select(_ => _closeButton.UpdateAsObservable()).Take(1).Subscribe(_ => Hide()).AddTo(gameObject);
        }
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _socialView.SetData();
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