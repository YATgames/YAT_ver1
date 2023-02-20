using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI;
using System.Threading;
using System;
using Assets.Scripts.Manager;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

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
            Debug.Log("PopupManger : Style : " + style);
            var popupName = GetPopupName(style); // 팝업 이름으로 UIPopu(Style) 
            var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style)); // popupBase 는 생성된 객체가 됨;
            Debug.Log("popupBaseType :" + popupBase.GetType().ToString());
            if (popupBase == null) // 리스트에 해당하는 PopupStyle이 없음)
            {
                // 그럼 추가해줘야지
                var resource = Resources.LoadAsync<GameObject>(popupName);

                while (!resource.isDone) // isDone은 LoadAsync가 완료됨을 반환함
                {
                    if (cancelltationToken.IsCancellationRequested)
                        yield break;
                    yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                }

                popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>(); // 생성시킬 위치

                var popupCanvas = popupBase.GetComponentInChildren<Canvas>(); // poupBase의 자식객체의 canvas
                if (popupBase as PopupSub) // SubPopup이라면 카메라가 필요없음
                {
                    Debug.Log("<color=yellow>캔버스생성 if - overlay</color>");
                    popupCanvas.worldCamera = null; // 생성되는 캔버스에 카메라 없애기? 
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    Debug.Log("<color=yellow>캔버스생성 else - camera</color>");
                    popupCanvas.worldCamera = _cameraManager.Camera; // 카메라매니저에서 설정한 카메라 가져옴
                    popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                }
                popupBase.transform.SetParent(transform);
                //Debug.Log("1");
                popupBase.transform.localScale = Vector3.one;
                popupBase.transform.localPosition = Vector3.zero;
                //popupBase.gameObject.SetActive(false);
                popupBase.gameObject.SetActive(true);
                PopupList.Add(popupBase);
                //Debug.Log("2");
            }
            //Debug.Log("poupBase이름 : " + popupBase.gameObject.name);
            // InvalidCastException: Specified cast is not valid.발생하게 됨       
            yield return popupBase;
        }
        private IEnumerator Get<T>(IObserver<T> observer, CancellationToken cancelltationToken, PopupStyle style) where T : PopupBase
        {
            Debug.Log("Get부분");
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancelltationToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
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
            Debug.Log("Show부분 : UIPopupMain이 이 부분을 호출한다");

            PopupBase popupBase = null; // null 로 만든 뒤 아래 Observable에서 할당이 완료 되면 반환하도록 함
            // UIPopupMain에서 PopupBase접근이 안되나?

            yield return Observable.FromCoroutineValue<T>(()
                => Get<T>(cancelltaionToken, style))
                .Where(popup => popup != null)
                .StartAsCoroutine(popup => popupBase = popup);


            //yield return Get<T>(cancelltaionToken, style);

            // 반환하는 과정에서 문제가 생김
            Debug.Log("<color=white>Get 부분 넘김</color>");
            if (popupBase != null)
            {
                Debug.Log("popoupBase가 존재함!");
                popupBase.SetParent(transform); // 이 부분을 위해서 popupBase가 전달이 되어야함
                popupBase.Show(data);
                popupBase.gameObject.SetActive(true);

                observer.OnNext(popupBase.GetComponent<T>());
                observer.OnCompleted();
            }
            else
            {
                Debug.Log("popoupBase = null");
            }

        }
        public bool IsShow(PopupStyle style) // 파라미터로 들어오는 팝업이 활성화 되어있는지 반환하기
        {
            Debug.Log("isShow");
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
        public List<PopupBase> GetShowingPopupList()
        {
            Debug.Log("GetShowingPopupList");
            return PopupList.Where(@base => @base.IsActive).ToList();
        }
        public List<PopupSub> GetshowingSubPopupList()
        {
            return PopupList.Where(@base => (@base as PopupSub) && @base.IsActive).Select(@base => (PopupSub)@base).ToList();
        }
    }
}