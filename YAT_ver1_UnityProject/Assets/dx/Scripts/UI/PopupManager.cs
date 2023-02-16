using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI;
using System.Threading;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UIElements;
using Assets.Scripts.Manager;

namespace Assets.Scripts.UI
{
    public class PopupManager : UnitySingleton<PopupManager>
    {
        // 싱글톤으로 생성시키는 객체. 기능들 여기에 구현됨.

        // = Constant
        private const string PopupPrefix = "Prefs/Popup/UIPopup";

        // = Field
        public readonly List<PopupBase> PopupList = new List<PopupBase>();

        [DependuncyInjection(typeof(CameraManager))]
        private CameraManager _cameraManager;

        public readonly OnEventTrigger<PopupBase> LoadCompletePopup 
            = new OnEventTrigger<PopupBase>();

        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);
        }
        public override void UnInitialize()
        {
            //base.UnInitialize();
            foreach (var popup in PopupList)
            {
                Destroy(popup.gameObject);
            }
            PopupList.Clear();
        }

        // = Construct
        // Get
        public IObservable<T> Get<T>(PopupStyle style) where T : PopupBase
        {
            return Observable.FromCoroutine<T>((observer, cancelltaionToken) => Get(observer, cancelltaionToken, style));
        }
        private IEnumerator Get<T>(CancellationToken cancelltationToken, PopupStyle style) where T : PopupBase
        {
            var popupName = GetPopupName(style); // 팝업 이름을 style로 가져와?
            // Splash , 
            var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style));
            if(popupBase == null) // 리스트에 해당하는 PopupStyle이 없음)
            {
                // 그럼 추가해줘야지
                Debug.Log("프리팹 로드단계");
                var resource = Resources.LoadAsync<GameObject>(popupName);

                while(!resource.isDone)
                {
                    if (cancelltationToken.IsCancellationRequested)
                        yield break;
                    yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                }

                popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>(); // Instantiate 위치
                var popupCanvas = popupBase.GetComponentInChildren<Canvas>(); // poupBase의 자식객체의 canvas
                if(popupBase as PopupSub)
                {
                    Debug.Log("캔버스생성 if - overlay");
                    popupCanvas.worldCamera = null; // 생성되는 캔버스에 카메라 없애기? 
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    Debug.Log("캔버스생성 else - camera");
                    popupCanvas.worldCamera = _cameraManager.Camera; // 카메라매니저에서 설정한 카메라 가져옴
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
        private IEnumerator Get<T>(IObserver<T> observer, CancellationToken cancelltationToken, PopupStyle style) where T : PopupBase
        {
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>( () => Get<T>(cancelltationToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
            observer.OnNext(popupBase.GetComponent<T>());
            observer.OnCompleted();
        }

        // Show 
        #region ::::: Show
        public IObservable<T> Show<T>(PopupStyle style, params object[] data) where T : PopupBase // 외부에서 불러오는 용도의 간소화된 함수
        {
            return Observable.FromCoroutine<T>((observer, cancelltaionToken) => Show(observer, cancelltaionToken, style, data));
        }
        private IEnumerator Show<T>(IObserver<T> observer, CancellationToken cancelltaionToken, PopupStyle style, object[] data) where T : PopupBase // 내부에서 동작하는 Token 검사하는 함수        
        {
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancelltaionToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
        }
        public bool IsShow(PopupStyle style) // 파라미터로 들어오는 팝업이 활성화 되어있는지 반환하기
        {
            var popup = PopupList.Find(pop => pop.name.Equals(GetPopupName(style))); // 현재 팝업스타일 가져오기
            return popup != null && popup.gameObject.activeSelf;
        }
        #endregion


        #region ::::: Hide
        public void Hide(PopupStyle style)
        {
            var popup = PopupList.Find(child => child.PopupStyle.Equals(style));
            if (popup != null && popup.gameObject.activeSelf)
                popup.Hide();
        }
        #endregion

        // = Method
        private string GetPopupName(PopupStyle style) // 팝업 이름 가져오기
        {
            return string.Format("{0}{1}", PopupPrefix, style);
        }
    }
}