using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class PopupManager : UnitySingleton<PopupManager>
	{
		// = Constant =============================================================================
		private const string PopupPrefix = "Prefs/Popup/UIPopup";

		// = Field ================================================================================
		public readonly List<PopupBase> PopupList = new List<PopupBase>();

		[DependuncyInjection(typeof(CameraManager))]
		private CameraManager _cameraManager;

		public readonly OnEventTrigger<PopupBase> LoadCompletePopup = new OnEventTrigger<PopupBase>();

		// = Construct ============================================================================
		// = Get / Set ============================================================================

		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);
		}

		public override void UnInitialize()
		{
			foreach (var popup in PopupList)
			{
				Destroy(popup.gameObject);
			}
			PopupList.Clear();
		}

		private IEnumerator Get<T>(CancellationToken cancellationToken, PopupStyle style) where T : PopupBase
		{
			var popupName = GetPopupName(style);
			var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style));
			if (popupBase == null)
			{
				//				var loadingSize = typeof(T) == typeof(PopupBase) ? SystemLoading.LoadingSize.Big : SystemLoading.LoadingSize.Small;
				var resource = Resources.LoadAsync<GameObject>(popupName);

				//if (typeof(T) == typeof(PopupBase) == false)
				//	SystemLoading.Show(SystemLoading.LoadingSize.Small, this);

				while (!resource.isDone)
				{
					if (cancellationToken.IsCancellationRequested)
						yield break;
					yield return FrameCountType.FixedUpdate.GetYieldInstruction();
				}

				popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>();

				var popupCanvas = popupBase.GetComponentInChildren<Canvas>();
				if (popupBase as PopupSub)
				{
					popupCanvas.worldCamera = null;
					popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				}
				else
				{
					popupCanvas.worldCamera = _cameraManager.Camera;
					popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
				}
				popupBase.transform.SetParent(transform);
				popupBase.transform.localScale = Vector3.one;
				popupBase.transform.localPosition = Vector3.zero;
				popupBase.gameObject.SetActive(false);
				PopupList.Add(popupBase);
			}

			yield return popupBase;
		}

		private IEnumerator Get<T>(IObserver<T> observer, CancellationToken cancellationToken, PopupStyle style) where T : PopupBase
		{
			PopupBase popupBase = null;
			yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancellationToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
			observer.OnNext(popupBase.GetComponent<T>());
			observer.OnCompleted();
		}

		public IObservable<T> Get<T>(PopupStyle style) where T : PopupBase
		{
			return Observable.FromCoroutine<T>((observer, cancellationToken) => Get(observer, cancellationToken, style));
		}

		private IEnumerator Show<T>(IObserver<T> observer, CancellationToken cancellationToken, PopupStyle style, object[] data) where T : PopupBase
		{
			PopupBase popupBase = null;
			yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancellationToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
			popupBase.SetParent(transform);
			popupBase.Show(data);
			observer.OnNext(popupBase.GetComponent<T>());
			observer.OnCompleted();
		}

		public IObservable<T> Show<T>(PopupStyle style, params object[] data) where T : PopupBase
		{
			return Observable.FromCoroutine<T>((observer, cancellationToken) => Show(observer, cancellationToken, style, data));
		}

		public void Hide(PopupStyle style)
		{
			var popup = PopupList.Find(child => child.PopupStyle.Equals(style));
			if (popup != null && popup.gameObject.activeSelf)
				popup.Hide();
		}

		public bool IsShow(PopupStyle style)
		{
			var popup = PopupList.Find(pop => pop.name.Equals(GetPopupName(style)));
			return popup != null && popup.gameObject.activeSelf;
		}

		// = Method ============================================================================
		private string GetPopupName(PopupStyle style)
		{
			return string.Format("{0}{1}", PopupPrefix, style);
		}

		public List<PopupBase> GetShowingPopupList()
		{
			return PopupList.Where(@base => @base.IsActive).ToList();
		}

		public List<PopupSub> GetShowingSubPopupList()
		{
			return PopupList.Where(@base => (@base as PopupSub) && @base.IsActive).Select(@base => (PopupSub)@base).ToList();
		}

		public bool EscapeHideSubPopup()
		{
			var hideSubPopup = GetShowingSubPopupList().LastOrDefault();
			if (!hideSubPopup || hideSubPopup.IsIgnoreEscapeHide) return false;
			hideSubPopup.Hide();
			return true;
		}
	}
}
