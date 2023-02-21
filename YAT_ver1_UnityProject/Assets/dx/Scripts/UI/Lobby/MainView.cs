using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI; // DependuncyInjection
using UniRx;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class MainView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public  SoundManager SoundManager { get; set; }
        [SerializeField] private Button _socialButton;
        [SerializeField] private Button _battleButton;

        //private OnEventTrigger<int> _oncClick = new OnEventTrigger<int>();
        // 아마 로비에서 케이스 클릭하는 부분 핟당한거 같은데 쓸일없어보임

        //private List<int> _datas;
        private void Start()
        {
            AddEvent();
            DependuncyInjection.Inject(this);
        }
        private void AddEvent()
        {
            if(_socialButton != null)
            {
                _socialButton.onClick.AsObservable().Subscribe(_ =>
                {
                    Debug.Log("Social Buttoin click");
                    FlowManager.AddSubPopup(PopupStyle.Social); // SubPop추가
                }).AddTo(gameObject);
            }


            _battleButton.OnClickAsObservable().Subscribe(_ =>
            {
                FlowManager.Change(PopupStyle.Battle); // Change
            }).AddTo(gameObject);


            /*
            _socialButton.OnClickAsObservable().Subscribe(v_ => FlowManager.AddSubPopup(PopupStyle.Social))
                .AddTo(gameObject);*/
        }
    
        public void SetData()
        {
            //var data =  datas.FirstOrDefault(v => )
            Debug.Log("[MainView] 데이터 설정 model -> view 로 전달할것들");
        }
    }
}