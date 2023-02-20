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
        // �̱������� ������Ű�� ��ü. ��ɵ� ���⿡ ������.

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
            var popupName = GetPopupName(style); // �˾� �̸����� UIPopu(Style) 
            var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style)); // popupBase �� ������ ��ü�� ��;
            Debug.Log("popupBaseType :" + popupBase.GetType().ToString());
            if (popupBase == null) // ����Ʈ�� �ش��ϴ� PopupStyle�� ����)
            {
                // �׷� �߰��������
                var resource = Resources.LoadAsync<GameObject>(popupName);

                while (!resource.isDone) // isDone�� LoadAsync�� �Ϸ���� ��ȯ��
                {
                    if (cancelltationToken.IsCancellationRequested)
                        yield break;
                    yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                }

                popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>(); // ������ų ��ġ

                var popupCanvas = popupBase.GetComponentInChildren<Canvas>(); // poupBase�� �ڽİ�ü�� canvas
                if (popupBase as PopupSub) // SubPopup�̶�� ī�޶� �ʿ����
                {
                    Debug.Log("<color=yellow>ĵ�������� if - overlay</color>");
                    popupCanvas.worldCamera = null; // �����Ǵ� ĵ������ ī�޶� ���ֱ�? 
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    Debug.Log("<color=yellow>ĵ�������� else - camera</color>");
                    popupCanvas.worldCamera = _cameraManager.Camera; // ī�޶�Ŵ������� ������ ī�޶� ������
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
            //Debug.Log("poupBase�̸� : " + popupBase.gameObject.name);
            // InvalidCastException: Specified cast is not valid.�߻��ϰ� ��       
            yield return popupBase;
        }
        private IEnumerator Get<T>(IObserver<T> observer, CancellationToken cancelltationToken, PopupStyle style) where T : PopupBase
        {
            Debug.Log("Get�κ�");
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancelltationToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);
            observer.OnNext(popupBase.GetComponent<T>());
            observer.OnCompleted();
        }

        // Show 
        #region ::::: Show
        public IObservable<T> Show<T>(PopupStyle style, params object[] data) where T : PopupBase // �ܺο��� �ҷ����� �뵵�� ����ȭ�� �Լ�
        {
            return Observable.FromCoroutine<T>((observer, cancelltaionToken) => Show(observer, cancelltaionToken, style, data));
        }
        private IEnumerator Show<T>(IObserver<T> observer, CancellationToken cancelltaionToken, PopupStyle style, object[] data) where T : PopupBase // ���ο��� �����ϴ� Token �˻��ϴ� �Լ�        
        {
            Debug.Log("Show�κ� : UIPopupMain�� �� �κ��� ȣ���Ѵ�");

            PopupBase popupBase = null; // null �� ���� �� �Ʒ� Observable���� �Ҵ��� �Ϸ� �Ǹ� ��ȯ�ϵ��� ��
            // UIPopupMain���� PopupBase������ �ȵǳ�?

            yield return Observable.FromCoroutineValue<T>(()
                => Get<T>(cancelltaionToken, style))
                .Where(popup => popup != null)
                .StartAsCoroutine(popup => popupBase = popup);


            //yield return Get<T>(cancelltaionToken, style);

            // ��ȯ�ϴ� �������� ������ ����
            Debug.Log("<color=white>Get �κ� �ѱ�</color>");
            if (popupBase != null)
            {
                Debug.Log("popoupBase�� ������!");
                popupBase.SetParent(transform); // �� �κ��� ���ؼ� popupBase�� ������ �Ǿ����
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
        public bool IsShow(PopupStyle style) // �Ķ���ͷ� ������ �˾��� Ȱ��ȭ �Ǿ��ִ��� ��ȯ�ϱ�
        {
            Debug.Log("isShow");
            var popup = PopupList.Find(pop => pop.name.Equals(GetPopupName(style))); // ���� �˾���Ÿ�� ��������
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
        private string GetPopupName(PopupStyle style) // �˾� �̸� ��������
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