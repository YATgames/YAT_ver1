using Assets.Scripts.Common.DI;
using UnityEngine;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using DXApp_AppData.Table;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.Util;

namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupDailyCheck : PopupSub
    {
        [SerializeField] private Button _okayButton;


        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _okayButton.OnClickAsObservable("Button_Touch").Subscribe(v => Hide()).AddTo(gameObject);
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
