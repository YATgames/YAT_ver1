using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI; // DependuncyInjection
using System.Runtime.InteropServices;
using System.Linq;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class LobbyView : MonoBehaviour
    {
        public ResourcesManager ResourcesManager { get; set; }
        public  SoundManager SoundManager { get; set; }
        [SerializeField] private Button _expalinButton;

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

        }
    
        public void SetData()
        {
             //var data =  datas.FirstOrDefault(v => )
        }
    }
}