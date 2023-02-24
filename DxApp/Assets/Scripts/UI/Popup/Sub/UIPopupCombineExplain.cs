using Assets.Scripts.Common.DI;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Util;


namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupCombineExplain : PopupSub
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [SerializeField] private Button _closeButton;

        [SerializeField] private CombineExplainView _combineExplainView;
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _combineExplainView.FlowManager = _flowManager;
        }
        public void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            _closeButton.OnClickAsObservable("Button_Click").Select(_ => _closeButton.UpdateAsObservable()).Take(1).Subscribe(_ => Hide()).AddTo(gameObject);
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
