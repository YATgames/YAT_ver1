using Assets.Scripts.Common.DI;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using Assets.Scripts.UI.Popup.PopupView;
using Assets.Scripts.Managers;
using System.Collections.Generic;

namespace Assets.Scripts.UI.Popup.Sup
{
    public class UIPopupAchieveSynergyInfo : PopupSub
    {
        [SerializeField] private Button _closeButton;

        [SerializeField] private AchiveSynergyInfoView _achiveSynergyInfoView;

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);
        }
        public void Start()
        {
            AddEvent();
        }
        private void AddEvent()
        {
            _closeButton.OnClickAsObservable().Subscribe(_ => Hide()).AddTo(gameObject);
        }
        public override void Show(params object[] data)
        {
            base.Show(data);
            List<string> _sysnergyPartsData = data[0] as List<string>;
            _achiveSynergyInfoView.SetData(_sysnergyPartsData);
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
