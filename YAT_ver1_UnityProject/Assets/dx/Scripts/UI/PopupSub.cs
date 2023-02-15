using UnityEngine;
using UniRx;
using Assets.Scripts.Util; // OnPointerDownAsObservable

namespace Assets.Scripts.UI
{
    public class PopupSub : PopupBase
    {
        protected bool BackgroundHideCheckLocked { get; set; } // ����� ����ִ��� Ȯ���ϴ� �Ӽ�

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


        // ���� EventExtensions�� �մ� �Լ��ε� �� �Լ��� ���� ���ϰ�� �ȵǳ�?

    }
}