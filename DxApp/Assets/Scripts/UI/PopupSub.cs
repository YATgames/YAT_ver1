using Assets.Scripts.Util;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class PopupSub : PopupBase
	{
		protected bool BackgoundHideCheckLocked { get; set; }

		public virtual void SetHideCheckTransform(Transform hideCheckTransform)
		{
			if (hideCheckTransform != null)
			{
				hideCheckTransform.OnPointerDownAsObservable().Where(_ => !BackgoundHideCheckLocked).Subscribe(_ => Hide()).AddTo(gameObject);
			}
		}

		protected override void OnDestroy()
		{
			Hide();
			UnInitialize();
		}
	}
}