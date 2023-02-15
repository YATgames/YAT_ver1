using Assets.Scripts.Managers;
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

        private EventExtension.OnEventTrigger<int> _oncClick = new EventExtension.OnEventTrigger<int>();
        // �Ƹ� �κ񿡼� ���̽� Ŭ���ϴ� �κ� �����Ѱ� ������ ���Ͼ����

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
                Debug.Log("�׽�Ʈ ��ư ����");
                // FlowManager. ���⼭ Change �Լ��� ��
            }).AddTo(gameObject);

            _testButton.OnClickAsObservable().Subscribe(v_ => FlowManager.AddSubPopup(PopupStyle.Test))
                .AddTo(gameObject);
        }
    
        public void SetData()
        {
             //var data =  datas.FirstOrDefault(v => )
        }
    }
}