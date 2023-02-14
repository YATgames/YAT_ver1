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
    public class ManageObjectFacade // 싱글톤들 관리만 하는 클래스?? // 인스턴스화 안해도 됨
    {
        private static bool _isInitialize;
        
        // 플로우매니저는 사용해야하나?
        public static PopupManager PopupManager { get { return PopupManager.Instance; } }

        private static IDisposable _gameServerConnectorDIssposable = null; // 서버연결? 안씀

        public static void Initialize()
        {
            if (_isInitialize)
                return;

            SoundManager.Instance.Initialize();

            _isInitialize = true;
            GameManager.Instance.Initialize();
            PlayerViewModel.Instance.Initialize();

            PopupManager.Instance.Initialize();

            // 카메라 매니저 초기화
        }
        public static IObservable<Unit> UnInitialize() // FromCorouitn 머 야이거?
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
                yield return FrameCountType.FixedUpdate.GetYieldInstruction(); // 이건 또 무슨구문임? ee

                _isInitialize = false;
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
                
            }
        }

    }
}
