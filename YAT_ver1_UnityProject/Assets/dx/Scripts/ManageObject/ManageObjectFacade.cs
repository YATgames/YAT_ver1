using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.UI;
using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Common.Models;
using System.Threading;

namespace Assets.Scripts.ManageObject
{
    public class ManageObjectFacade // �̱���� ������ �ϴ� Ŭ����?? // �ν��Ͻ�ȭ ���ص� ��
    {
        private static bool _isInitialize;
        
        // �÷ο�Ŵ����� ����ؾ��ϳ�?
        public static PopupManager PopupManager { get { return PopupManager.Instance; } }

        private static IDisposable _gameServerConnectorDIssposable = null; // ��������? �Ⱦ�

        public static void Initialize()
        {
            if (_isInitialize)
                return;

            SoundManager.Instance.Initialize();

            _isInitialize = true;
            GameManager.Instance.Initialize();
            PlayerViewModel.Instance.Initialize();

            PopupManager.Instance.Initialize();

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
                PlayerViewModel.Instance.UnInitialize();
                SoundManager.Instance.UnInitialize();
                ResourcesManager.Instance.UnInitialize();
                GameManager.Instance.UnInitialize();
                yield return FrameCountType.FixedUpdate.GetYieldInstruction(); // �̰� �� ����������? ee

                _isInitialize = false;
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
                
            }
        }

    }
}
