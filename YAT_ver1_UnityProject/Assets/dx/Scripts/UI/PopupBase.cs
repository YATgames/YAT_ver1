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

                var popupName = gameObject.name; // 호출 대상의 이름 가져옴
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


        protected GameObject Camera; // 메인 카메라
        protected CompositeDisposable CancelerObject; // 어따씀이거 UniRx 멤버인데
        // 여러 이벤트를 한번에 이 클래스를 통해 한번에 UnSubscribe(Destory) 한다
        // Observable을 안드로이드 라이프 사이클에 맞춰서 한번에 모두 메모리를 해제할 수 있다.

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
        /// 화면을 출력한다
        /// </summary>
        /// <param name="data"></param>
        public virtual void Show(params object[] data)
        {
            Debug.Log("<color=yellow>PopupBase : Show</color>");
            if (CancelerObject != null)
                CancelerObject.Dispose();

            Debug.Log(data + "혹은 " +gameObject +" : 활성화");
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
                PopupManager.Instance.PopupList.Remove(this); // 활성화된 팝업이 있다면 지우고
            Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            UnInitialzie();
            Resources.UnloadUnusedAssets();
        }

        // parent 가 camera를 따로 가지고 있다면 renderMode 및 worldCamera 를 교체해주기 위한 메소드
        // (raycast에대한 enable속성도 이곳에서 설정한다)
        public void SetParent(Transform parent) // 부모객체 설정. 카메라에서 사용됨
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }
    }
}