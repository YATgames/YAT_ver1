using System;
using System.Collections;
using System.Threading;
using Assets.Scripts.Common;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Util
{
	public class ChangeResolution : UnitySingleton<ChangeResolution>
	{
		public Vector2 ScreenSize { get; set; }
		void Awake()
		{
			ResizeWindow().Subscribe(size => ScreenSize = size).AddTo(gameObject);
		}

		private IObservable<Vector2> ResizeWindow()
		{
			return Observable.FromCoroutine<Vector2>(CheckResize);
		}

		private IEnumerator CheckResize(IObserver<Vector2> observer, CancellationToken cancellationToken)
		{
			var isPublish = false;
			var screenSize = new Vector2(Screen.width, Screen.height);
			try
			{
				if (cancellationToken.IsCancellationRequested) yield break;

				if (!screenSize.Equals(ScreenSize))
				{
					isPublish = true;
				}
			}
			catch (Exception e)
			{
				observer.OnError(e);
			}
			yield return FrameCountType.FixedUpdate.GetYieldInstruction();

			if (isPublish)
				observer.OnNext(screenSize);

			yield return CheckResize(observer, cancellationToken);
		}
	}
}