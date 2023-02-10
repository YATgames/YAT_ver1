using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Util
{
	public static class DOTweenEX {

		#region :::::::: Fade In Out 
		public static IEnumerator Co_FadeInOut(this Image img , float time)
		{
			ReactiveProperty<bool> complete = new ReactiveProperty<bool>();
			complete.Value = false;
			Sequence sequence = DOTween.Sequence()
				.SetAutoKill(true)
				.Append(img.DOFade(1f, time))
				.Append(img.DOFade(0f, time))
				.OnComplete(() => { complete.Value = true; });

			yield return complete
				.ObserveEveryValueChanged(v => v.Value)
				.Where(v => v)
				.Take(1)
				.StartAsCoroutine();
		}

		public static void FadeInOut(this Image img , float time)
		{
			Sequence sequence = DOTween.Sequence()
				.SetAutoKill(true)
				.Append(img.DOFade(1f, time))
				.Append(img.DOFade(0f, time));
		}
		#endregion

		#region :::::::: Update UI
		public static TweenerCore<float, float , FloatOptions> UpdateValue(this Text text , float value , float endValue, float duaration)
		{
			return DOTween.To(() => value, f => value = f, endValue, duaration).OnUpdate(() =>
			{
				text.text = value.ToString();
			});
		}

		public static TweenerCore<long, long, NoOptions> UpdateValue(this Text text, long value, long endValue, float duaration)
		{
			return DOTween.To(() => value, l => value = l, endValue, duaration).OnUpdate(() =>
			{
				text.text = value.ToString();
			});
		}
		#endregion

		#region :::::::: To
		public static void DoLocalTo(this Transform tr, Vector3 endPos, float duration)
		{
			tr.DOLocalMove(endPos, duration);
		}

		#endregion

		#region :::::::: From
		public static void DoLocalFrom(this Transform tr , Vector3 startPos , float duration)
		{
			var pos = tr.transform.localPosition;
			tr.localPosition = new Vector3(pos.x + startPos.x , pos.y + startPos.y , pos.z + startPos.z);
			tr.DOLocalMove(pos, duration);
		}
		#endregion

	}
}
