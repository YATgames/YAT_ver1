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



        protected CompositeDisposable CancelerObject; // 어따씀이거 UniRx 멤버인데
        // 여러 이벤트를 한번에 이 클래스를 통해 한번에 UnSubscribe(Destory) 한다.

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
        /// 화면을 출력한다
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
                PopupManager.Instance.PopupList.Remove(this); // 활성화된 팝업이 있다면 지우고
            Destroy(gameObject);
        }
        protected virtual void OnDestory()
        {
            UnInitialzie();
            Resources.UnloadUnusedAssets();
        }
    }
}