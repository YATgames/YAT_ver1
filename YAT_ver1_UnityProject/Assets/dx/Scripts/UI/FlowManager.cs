using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Runtime.CompilerServices;

namespace Assets.Scripts.UI
{
    public class FlowManager : UnitySingleton<FlowManager>
    {
        // 팝업들 불러오는 역할을 하는 스크립트
        private readonly List<FlowNode> _listNode = new List<FlowNode>();
        public PopupStyle CurStyle { get; private set; } = PopupStyle.None;

        // Get / Set
        public FlowNode AddFlow(PopupStyle style, params object[] data)
        {
            // 새로운 노드를 넣고 그걸 반환시킴
            Push(new FlowNode(style, data));
            return GetLastNode();
        }
        public FlowNode GetLastNode() // 마지막 노드를 반환
        {
            // 현재 FlowNode의 개수가 1이상이라면 개수의 -1(배열이니까) 반환, 0이면 null 반환
            return (_listNode.Count > 0 ? _listNode[_listNode.Count - 1] : null);
        }

        public IObservable<T> AddsubPopup<T>(PopupStyle style, params object[] data) where T : PopupSub // AddSubPopup인데 IObservable를 반환함
        {
            return PopupManager.Instance.Show<T>(style, data);
        }

        #region ::::: AddPopup
        public void AddSubPopup(PopupStyle style, params object[] data) //
        {
            Debug.Log("<color=black>AddSubPopup</color>");
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }
       public void AddContentsPopup(PopupStyle style, params object[] data)
        {
            Debug.Log("<color=black>AddContentsPopup</color>");
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }
        #endregion

        #region ::::: Change
        public void Change(PopupStyle style, params object[] data)
        {
            if (CurStyle == style && style != PopupStyle.None)
                return;

            var node = GetLastNode();
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true); // 활성화 시키면서 다른거 다 지우기
            CurStyle = style;

            if (style == PopupStyle.None)
                return;

            Debug.Log("AddFlow :" + style+ "+ " + data);
            AddFlow(style, data);
            PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
        }
        // 인터페이스 Observable
        public IObservable<T> Change<T>(PopupStyle style, params object[] data) where T :PopupBase
        {
            if (CurStyle == style && style != PopupStyle.None)
                return NonePopupObservable<T>(); // Change를 사용할떄 가져온 <T> 형태 그대로 반환
            var node = GetLastNode(); // 현재 마지막(사용중인) 노드 가져akwlakr오기

            // LastNode로 받아온 node가 존재한다면 style를 지우고 없다면 현재 curStyle 를 지운다.
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true);
            CurStyle = style;

            if (style == PopupStyle.None)
                return NonePopupObservable<T>();

            AddFlow(style, data); // Flow 에 추가함
            return PopupManager.Instance.Show<T>(style, data);
        }

        #endregion

        private IObservable<T> NonePopupObservable<T>() where T : PopupBase
        {
            var observer = new Subject<T>(); // Subject를 탬플릿 형태로 선언한 observer 선언
            Observable.NextFrame().Subscribe(_ =>
            {
                observer.OnNext(null);
                observer.OnCompleted();
            }).AddTo(gameObject);
            return observer;
        }

        private void AllHideSubPopup(bool isForce)
        {
            var popupList = PopupManager.Instance.GetShowingPopupList();
            foreach (var subPopup in popupList)
            {
                if(isForce)
                {
                    subPopup.Hide(); // 보조 팝업들 지우기
                }
                else
                {
                    if(!subPopup.IsIgnoreEscapeHide)
                    {
                        subPopup.Hide();
                    }
                }
            }
        }

        // = Method
        private void Push(FlowNode nextNode)
        {
            if (nextNode == null) // 파라미터로 넣는 nextNode가 null이다 = Prefs 에 해당UI 프리팹이 없음
            {
                _listNode.Clear();
                return;
            }
            int index = _listNode.FindIndex(node => node.Style.Equals(nextNode.Style)); // FindIndex 함수의 인자 : startindex, count ,predicatd 

            if(index != -1) // 중복된 노드가 있을 경우 그 노드부터 이후로 제거 // -1인 이유 몰루
            {
                Debug.Log("현재 리스트 노드의 개수" + _listNode.Count);
                // 기존에 있던 노드 지우기인데 노드가 비어있음
                _listNode.RemoveRange(index, (_listNode.Count - index)); 
            }
            Debug.Log("Add이전");
            _listNode.Add(nextNode);
        }

        // = SubClass 
        public class FlowNode
        {
            public PopupStyle Style { get; private set; } // 스타일을 가져오는건 맘대로, 바꾸지는 못하게
            public object[] Data { get; set; }

            public FlowNode(PopupStyle style, params object[] data)
            {
                Style = style;
                if (data != null && data.Length > 0) // object인 data 가 이미 있는 상황이라면
                {
                    if (data[0] != null)
                    {
                        Data = null; // 파라미터data의 값이 없다면 지역변수 Data도 null로 설정함
                    }
                }
            }
        }
    }
}