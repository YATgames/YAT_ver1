using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Common;
using Assets.Scripts.Util;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Managers;
using System.Threading;
using System;

namespace Assets.Scripts.UI
{
    public class PopupManager : UnitySingleton<PopupManager>
    {
        // �̱������� ������Ű�� ��ü. ��ɵ� ���⿡ ������.

        // = Constant
        private const string PopupPrefix = "Prefs/Popup/UIPopup";

        // = Field
        public readonly List<PopupBase> PopupList = new List<PopupBase>();

        // ī�޶� �Ŵ��� ���� ��������

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
            var popupName = GetPopupName(style); // �˾� �̸��� style�� ������?
            var popupBase = PopupList.Find(child => child.PopupStyle.Equals(style));
            if(popupBase == null) // ����Ʈ�� �ش��ϴ� PopupStyle�� ����)
            {
                // �׷� �߰��������
                var resource = Resources.LoadAsync<GameObject>(popupName);

                while(!resource.isDone)
                {
                    if (cancelltationToken.IsCancellationRequested)
                        yield break;
                    yield return FrameCountType.FixedUpdate.GetYieldInstruction();
                }

                popupBase = Instantiate((GameObject)resource.asset).GetComponent<PopupBase>(); // Instantiate ��ġ
                var popupCanvas = popupBase.GetComponentInChildren<Canvas>(); // poupBase�� �ڽİ�ü�� canvas
                if(popupBase as PopupSub)
                {
                    Debug.Log("ĵ�������� if - overlay");
                    popupCanvas.worldCamera = null; // �����Ǵ� ĵ������ ī�޶� ���ֱ�? 
                    popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    Debug.Log("ĵ�������� else - camera");
                    //popupCanvas.worldCamera = _
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
        public IObservable<T> Show<T>(PopupStyle style, params object[] data) where T : PopupBase // �ܺο��� �ҷ����� �뵵�� ����ȭ�� �Լ�
        {
            return Observable.FromCoroutine<T>((observer, cancelltaionToken) => Show(observer, cancelltaionToken, style, data));
        }
        private IEnumerator Show<T>(IObserver<T> observer, CancellationToken cancelltaionToken, PopupStyle style, object[] data) where T : PopupBase // ���ο��� �����ϴ� Token �˻��ϴ� �Լ�        
        {
            PopupBase popupBase = null;
            yield return Observable.FromCoroutineValue<T>(() => Get<T>(cancelltaionToken, style)).Where(popup => popup != null).StartAsCoroutine(popup => popupBase = popup);

        }


        // = Method
        private string GetPopupName(PopupStyle style) // �˾� �̸� ��������
        {
            return string.Format("{0}{1}", PopupPrefix, style);
        }
    }
}