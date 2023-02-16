using System;
using System.Security.Authentication.ExtendedProtection;
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


        protected GameObject Camera; // ���� ī�޶�
        protected CompositeDisposable CancelerObject; // ������̰� UniRx ����ε�
        // ���� �̺�Ʈ�� �ѹ��� �� Ŭ������ ���� �ѹ��� UnSubscribe(Destory) �Ѵ�
        // Observable�� �ȵ���̵� ������ ����Ŭ�� ���缭 �ѹ��� ��� �޸𸮸� ������ �� �ִ�.

        public bool IsActive
        {
            get { return gameObject.activeInHierarchy; }
        }
        public bool IsIgnoreEscapeHide { get; set; }

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
            PopupManager.Instance.LoadCompletePopup.Invoke(this);
        }

        /// <summary>
        /// ȭ���� ����Ѵ�
        /// </summary>
        /// <param name="data"></param>
        public virtual void Show(params object[] data)
        {
            Debug.Log("<color=yellow>PopupBase : Show</color>");
            if (CancelerObject != null)
                CancelerObject.Dispose();

            Debug.Log(data + "Ȥ�� " +gameObject +" : Ȱ��ȭ");
            CancelerObject = new CompositeDisposable(); 
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            Debug.Log("<color=yellow>PopupBase : Hide</color>");
            if(CancelerObject != null)
            {
                CancelerObject.Dispose();
                CancelerObject = null;
            }
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
        protected virtual void OnDestroy()
        {
            UnInitialzie();
            Resources.UnloadUnusedAssets();
        }

        // parent �� camera�� ���� ������ �ִٸ� renderMode �� worldCamera �� ��ü���ֱ� ���� �޼ҵ�
        // (raycast������ enable�Ӽ��� �̰����� �����Ѵ�)
        public void SetParent(Transform parent) // �θ�ü ����. ī�޶󿡼� ����
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }
    }
}