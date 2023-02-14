using Assets.Scripts.Common.DI;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Popup.PopupView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Popup.Base
{
    public class UIPopupLobby : PopupBase
    {
        [DependuncyInjection(typeof(SoundManager))]
        private SoundManager _soundManager;

        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager _resourcesManager;

        [SerializeField] private LobbyView _lobbyView;


        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);

            _lobbyView.SetData(); // view �� ������ ����
            // Ŀ�ؼ� �Ŵ��� �˻�����
            // ����� ����

            // ConnectionLogin �� ���۵Ǹ� ���⼭ �α��� �˻�
            
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