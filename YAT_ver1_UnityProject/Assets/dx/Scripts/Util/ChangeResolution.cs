using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class ChangeResolution : UnitySingleton<ChangeResolution>
    {
        public Vector2 ScreenSize { get; set; }

        private void Awake()
        {
            ResizeWindow().Subscribe(size => ScreenSize = size).AddTo(gameObject);
        }

        private IObservable<Vector2> ResizeWindow()
        {
            return Observable.FromCoroutine<Vector2>(CheckResize);
        }

        private IEnumerator CheckResize(IObserver<Vector2> observer, CancellationToken cancelltaionToken)
        {
            var isPublish = false;
            var screenSize = new Vector2(Screen.width, Screen.height);
            try
            {
                if (cancelltaionToken.IsCancellationRequested)
                    yield break;
                if (!ScreenSize.Equals(ScreenSize))
                {
                    isPublish = true;
                }
            }
            catch (Exception e)
            {
                observer.OnError(e);
            }
            yield return FrameCountType.FixedUpdate.GetYieldInstruction();

            if (isPublish) // 퍼블리싱 된 상태?
                observer.OnNext(screenSize);

            yield return CheckResize(observer, cancelltaionToken); // 콜백하는데?


        } 
    }
}