using System;
using System.Collections;
using System.Threading;
using Assets.Scripts.Common.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using UniRx;

namespace Assets.Scripts.ManageObject
{
    public static class ManageObjectFacade
    {
        private static bool _isInitialize;

        public static FlowManager FlowManager { get { return FlowManager.Instance; } }
        public static PopupManager PopupManager { get { return PopupManager.Instance; } }

        private static IDisposable _gameServerConnectorDisposable = null;

        public static void Initialize()
        {
            if (_isInitialize)
                return;

			StringTable.Init();
			SoundManager.Instance.Initialize();

			_isInitialize = true;

			GameManager.Instance.Initialize();
            ResourcesManager.Instance.Initialize();

			ItemManager.Instance.Initialize();
			ConfigManager.Instance.Initialize();
			ConnectionManager.Instance.Initialize();

			PlayerViewModel.Instance.Initialize();
            FlowManager.Initialize();
            PopupManager.Initialize();

            CameraManager.Instance.Initialize();
        }

        public static IObservable<Unit> UnInitialize()
        {
            return Observable.FromCoroutine<Unit>(UnInitialize);
        }

        private static IEnumerator UnInitialize(IObserver<Unit> observer, CancellationToken cancellationToken)
        {
            if (_isInitialize == false)
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
            else
			{
				FlowManager.UnInitialize();
                PopupManager.UnInitialize();
                CameraManager.Instance.UnInitialize();
                PlayerViewModel.Instance.UnInitialize();
				ConnectionManager.Instance.UnInitialize();
				ConfigManager.Instance.UnInitialize();
				SoundManager.Instance.UnInitialize();
				ItemManager.Instance.UnInitialize();
				ResourcesManager.Instance.UnInitialize();
                GameManager.Instance.UnInitialize();
                yield return FrameCountType.FixedUpdate.GetYieldInstruction();

                _isInitialize = false;
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
        }
    }
}