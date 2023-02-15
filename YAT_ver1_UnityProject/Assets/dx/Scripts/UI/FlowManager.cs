using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Scripts.UI
{
    public class FlowManager : UnitySingleton<FlowManager>
    {
        // 팝업들 불러오는 역할을 하는 스크립트같음

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
            return PopupManager.Instance.Show<T>(style, data); // 근데 궁금하네 이건 Depencuncy로 안받음? 서로 싱글톤끼리는 아닌건가
        }
        public void AddSubPopup(PopupStyle style, params object[] data) //
        {
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }

        
       public void AddContentsPopup(PopupStyle style, params object[] data)
        {
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }


        private void AllHideSubPopup(bool isForce = false)
        {
            //var popupList =PopupManager.Instance.getshowingsu
        }

        // = Method
        private void Push(FlowNode nextNode)
        {
            if (nextNode == null)
            {
                _listNode.Clear();
                return;
            
            }
            int index = _listNode.FindIndex(node => node.Style.Equals(nextNode.Style)); // FindIndex 함수의 인자 : startindex, count ,predicatd 

            if(index != 1) // 중복된 노드가 있을 경우 그 노드부터 이후로 제거
            {
                _listNode.RemoveRange(index, (_listNode.Count - index));
            }
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
                if (data != null && data.Length > 0) // object인 data 가 있을때만 동작
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