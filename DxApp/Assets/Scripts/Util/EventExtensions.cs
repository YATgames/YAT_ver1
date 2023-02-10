using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Util
{
	public static class EventExtensions
	{

		public static Coroutine StartAsCoroutine<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, TProperty value, CancellationToken cancellationToken = default(CancellationToken)) where TSource : class
		{
			return source.ObserveEveryValueChanged(propertySelector).Where(v => v.Equals(value)).Take(1).StartAsCoroutine(cancellationToken);
		}

		public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, T value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return source.Where(v => v.Equals(value)).Take(1).StartAsCoroutine(cancellationToken);
		}

		public static Coroutine StartAsCoroutine<T>(this IReadOnlyReactiveProperty<T> source, T value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return source.ObserveEveryValueChanged(v => v.Value).Where(v => v.Equals(value)).Take(1).StartAsCoroutine(cancellationToken);
		}

		// NOTE : Toggle, Button 등등..더블클릭 체크를 위해서 변경
		public static IObservable<IList<PointerEventData>> OnDoubleClick(this Selectable selectable, float bufferTime = 200)
		{
			var stream = selectable.OnPointerClickAsObservable().Buffer(selectable.OnPointerClickAsObservable().Throttle(TimeSpan.FromMilliseconds(bufferTime))).Where(x => x.Count >= 2);
			return stream;
		}

		public static IObservable<Unit> OnClickAsObservable(this Button button, string soundKey)
		{
			button.OnPointerDownAsObservable().Where(_ => button.interactable).Subscribe(_ =>
			{
				SoundManager.Instance.Play(soundKey);
			}).AddTo(button.gameObject);

			return button.OnClickAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerDownAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();

			var obseraver = component.gameObject.GetComponent<ObservablePointerDownTrigger>();

			if (obseraver == null)
				obseraver = component.gameObject.AddComponent<ObservablePointerDownTrigger>();

			return obseraver.OnPointerDownAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerClickAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return component.gameObject.GetComponent<ObservablePointerClickTrigger>().OnPointerClickAsObservable();
		}

		public static IObservable<PointerEventData> OnBeginDragAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return component.gameObject.GetComponent<ObservableBeginDragTrigger>().OnBeginDragAsObservable();
		}

		public static IObservable<PointerEventData> OnPointerUpAsObservable(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<PointerEventData>();
			return component.gameObject.GetComponent<ObservablePointerUpTrigger>().OnPointerUpAsObservable();
		}

		public static IDisposable SubscribeToText(this IObservable<string> source, TextMeshProUGUI text)
		{
			return source.SubscribeWithState(text, (x, t) => t.text = x);
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, TextMeshProUGUI text)
		{
			return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
		}

		public static IObservable<KeyCode> OnKeyDown(this Component component, params KeyCode[] keyCodes)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<KeyCode>();
#if UNITY_EDITOR
			if (keyCodes == null)
				keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
#endif
			return
				component.gameObject.UpdateAsObservable()
					.Select(_ => keyCodes.Where(Input.GetKeyDown).FirstOrDefault())
					.Where(keyCode => !keyCode.Equals(KeyCode.None));
		}
		public static IObservable<KeyCode> OnKey(this Component component, params KeyCode[] keyCodes)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<KeyCode>();

			return
				component.gameObject.UpdateAsObservable()
					.Select(_ => keyCodes.Where(Input.GetKey).FirstOrDefault())
					.Where(keyCode => !keyCode.Equals(KeyCode.None));
		}
		public static IObservable<Unit> OnPress(this Component component)
		{
			if (component == null || component.gameObject == null) return Observable.Empty<Unit>();

			return
				component.gameObject.UpdateAsObservable().Where(_ => Input.anyKeyDown);
		}

	}
	[Serializable]
	public class OnEventTrigger : UnityEvent { }
	[Serializable]
	public class OnEventTrigger<T> : UnityEvent<T> { }
}
