using Assets.Scripts.Common.DI;
//using Assets.Scripts.Common.ViewModel;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Popup.Sub
{
    public class UIPopupDisplayOption : PopupSub
    {
        //[DependuncyInjection(typeof(PlayerViewModel))]
        //private PlayerViewModel _playerViewModel;
     

        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;

        [DependuncyInjection(typeof(GameManager))]
        private GameManager _gameManager;

        //[SerializeField] private DisplayOptionView _optionView;
        [SerializeField] private Button _closeButton;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            //_settingsView.PlayerViewModel = _playerViewModel;
           // _optionView.FlowManager = _flowManager;
           // _optionView.GameManager = _gameManager;

            _closeButton.OnClickAsObservable().Subscribe(_ => Hide()).AddTo(gameObject);
        }

        public override void Show(params object[] data)
        {
            base.Show(data);
            AtRoot();
        }

        public void AtRoot()
        {
            this.gameObject.transform.position = transform.parent.position;
        }

    }
}
