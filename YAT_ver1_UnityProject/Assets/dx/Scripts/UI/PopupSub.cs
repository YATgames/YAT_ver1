using UnityEngine;
using UniRx;
using Assets.Scripts.Util; // OnPointerDownAsObservable

namespace Assets.Scripts.UI
{
    public class PopupSub : PopupBase
    {
        protected bool BackgroundHideCheckLocked { get; set; } // 배경이 잠겨있는지 확인하는 속성

        public virtual void SetHideCheckTransform(Transform hideCheckTransform)
        {
            if (hideCheckTransform != null)
            {
                //hideCheckTransform.OnPointerDownAsObservable().Where(_ => !BackgroundHideCheckLocked).Subscribe(_ => Hide()).AddTo(gameObject);
            }
                           
        }

        protected override void OnDestroy()
        {
            Hide();
            UnInitialzie();
        }


        // 원래 EventExtensions에 잇는 함수인데 저 함수문 구현 안하고는 안되나?

    }
}