using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PopupBase : MonoBehaviour
    {
        public bool DontDestroy;
        private PopupStyle _popupStyle;
        public PopupStyle PopupStyle
        {
            get
            {
                if (!_popupStyle.Equals(PopupStyle.None))
                    return _popupStyle;

                var popupName = gameObject.name; // ȣ�� ����� �̸� ������
                popupName = popupName.Replace("UIPopup", "");
                popupName = popupName.Replace("(Clone)", "");
                try
                {
                    _popupStyle = (PopupStyle)Enum.Parse(typeof(PopupStyle), popupName);
                }
                catch(Exception)
                {
                    _popupStyle = PopupStyle.None;
                }
                return _popupStyle;
            }
        }



        protected CompositeDisposable CancelerObject; // ������̰� UniRx ����ε�
        // ���� �̺�Ʈ�� �ѹ��� �� Ŭ������ ���� �ѹ��� UnSubscribe(Destory) �Ѵ�.

        private void Awake()
        {
            Initialize();
        }
        public virtual void Initialize()
        { }
        public virtual void UnInitialzie()
        { }

        protected virtual void LoadComplete()
        {

        }


        /// <summary>
        /// ȭ���� ����Ѵ�
        /// </summary>
        /// <param name="data"></param>
        public virtual void Show(params object[] data)
        {
            
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (gameObject != null)
                gameObject.SetActive(false);
            if (DontDestroy)
                return;

            Destroy();
        }

        private void Destroy()
        {
            if (PopupManager.Instance)
                PopupManager.Instance.PopupList.Remove(this); // Ȱ��ȭ�� �˾��� �ִٸ� �����
            Destroy(gameObject);
        }
        protected virtual void OnDestory()
        {
            UnInitialzie();
            Resources.UnloadUnusedAssets();
        }
    }
}