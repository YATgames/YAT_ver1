using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI; // DependuncyInjection
using System.Runtime.InteropServices;
using System.Linq;
using UniRx;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class MainView : MonoBehaviour
    {
        public FlowManager FlowManager { get; set; }
        public ResourcesManager ResourcesManager { get; set; }
        public  SoundManager SoundManager { get; set; }
        [SerializeField] private Button _testButton;

        private OnEventTrigger<int> _oncClick = new OnEventTrigger<int>();
        // 아마 로비에서 케이스 클릭하는 부분 핟당한거 같은데 쓸일없어보임

        private List<int> _datas;
        private void Start()
        {
            AddEvent();
            DependuncyInjection.Inject(this);
        }

        private void AddEvent()
        {
            _testButton.onClick.AsObservable().Subscribe(_ =>
            {
                Debug.Log("테스트 버튼 누름");
                // FlowManager. 여기서 Change 함수가 들어감
            }).AddTo(gameObject);

            _testButton.OnClickAsObservable().Subscribe(v_ => FlowManager.AddSubPopup(PopupStyle.Test))
                .AddTo(gameObject);
        }
    
        public void SetData()
        {
            //var data =  datas.FirstOrDefault(v => )
            Debug.Log("데이터 설정 model -> view 로 전달할것들");
        }
    }
}