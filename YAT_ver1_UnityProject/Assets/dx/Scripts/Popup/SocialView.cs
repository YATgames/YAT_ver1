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
            Debug.Log("<color=aqua>Social 활성화</color>");
        }

        // view 내장함수
        public void SetData()
        {
            Debug.Log("<color=aqua>SociaView 데이터 정렬화</color>");
        }

    }
}