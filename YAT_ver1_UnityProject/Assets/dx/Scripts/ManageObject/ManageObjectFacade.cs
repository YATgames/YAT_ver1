using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.UI;
using System;
using Assets.Scripts.Manager;
using Assets.Scripts.Common.Models;
using System.Threading;

namespace Assets.Scripts.ManageObject
{
    public class ManageObjectFacade // �̱���� ������ �ϴ� Ŭ����?? // �ν��Ͻ�ȭ ���ص� ��
    {
        private static bool _isInitialize;
        
        // �÷ο�Ŵ����� ����ؾ��ϳ�?
        public static FlowManager FlowManager { get { return FlowManager.Instance; } }
        public static PopupManager PopupManager { get { return PopupManager.Instance; } }

        private static IDisposable _gameServerConnectorDIssposable = null; // ��������? �Ⱦ�

        public static void Initialize()
        {
            if (_isInitialize)
                return;

            _isInitialize = true;
            SoundManager.Instance.Initialize();
            GameManager.Instance.Initialize();
            PlayerViewModel.Instance.Initialize();

            FlowManager.Initialize();
            PopupManager.Initialize();
            CameraManager.Instance.Initialize();

            // ī�޶� �Ŵ��� �ʱ�ȭ
        }
        public static IObservable<Unit> UnInitialize() // FromCorouitn �� ���̰�?
        {
            return Observable.FromCoroutine<Unit>(UnInitialize);
        }
        public static IEnumerator UnInitialize(IObserver<Unit> observer, CancellationToken cancellationToken)
        {
            if(_isInitialize == false)
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
            else
            {
                PopupManager.UnInitialize();
                FlowManager.UnInitialize();
                PlayerViewModel.Instance.UnInitialize();
                SoundManager.Instance.UnInitialize();
                ResourcesManager.Instance.UnInitialize();
                GameManager.Instance.UnInitialize();
                CameraManager.Instance.UnInitialize();
                yield return FrameCountType.FixedUpdate.GetYieldInstruction(); // �̰� �� ����������? ee

                _isInitialize = false;
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
                
            }
        }

    }
}
