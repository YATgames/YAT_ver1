using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public class SocialView : MonoBehaviour
    {
        public GameManager GemeManager { get; set; }

        [SerializeField] private string _string;

        private void Start()
        {
            AddEvent();
        }

        private void AddEvent()
        {
            Debug.Log("<color=aqua>Social Ȱ��ȭ</color>");
        }

        // view �����Լ�
        public void SetData()
        {
            Debug.Log("<color=aqua>SociaView ������ ����ȭ</color>");
        }

    }
}