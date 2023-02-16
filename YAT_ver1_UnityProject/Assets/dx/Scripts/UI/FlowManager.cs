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
        // �˾��� �ҷ����� ������ �ϴ� ��ũ��Ʈ����

        private readonly List<FlowNode> _listNode = new List<FlowNode>();
        public PopupStyle CurStyle { get; private set; } = PopupStyle.None;

        // Get / Set
        public FlowNode AddFlow(PopupStyle style, params object[] data)
        {
            // ���ο� ��带 �ְ� �װ� ��ȯ��Ŵ
            Push(new FlowNode(style, data));
            return GetLastNode();
        }
        public FlowNode GetLastNode() // ������ ��带 ��ȯ
        {
            // ���� FlowNode�� ������ 1�̻��̶�� ������ -1(�迭�̴ϱ�) ��ȯ, 0�̸� null ��ȯ
            return (_listNode.Count > 0 ? _listNode[_listNode.Count - 1] : null);
        }

        public IObservable<T> AddsubPopup<T>(PopupStyle style, params object[] data) where T : PopupSub // AddSubPopup�ε� IObservable�� ��ȯ��
        {
            return PopupManager.Instance.Show<T>(style, data); // �ٵ� �ñ��ϳ� �̰� Depencuncy�� �ȹ���? ���� �̱��波���� �ƴѰǰ�
        }

        #region ::::: AddPopup
        public void AddSubPopup(PopupStyle style, params object[] data) //
        {
            PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
        }
       public void AddContentsPopup(PopupStyle style, params object[] data)
        {
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
            AllHideSubPopup(true); // Ȱ��ȭ ��Ű�鼭 �ٸ��� �� �����
            CurStyle = style;

            if (style == PopupStyle.None)
                return;

            AddFlow(style, data);
            PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
        }
        // �������̽� Observable
        public IObservable<T> Change<T>(PopupStyle style, params object[] data) where T :PopupBase
        {
            if (CurStyle == style && style != PopupStyle.None)
                return NonePopupObservable<T>(); // Change�� ����ҋ� ������ <T> ���� �״�� ��ȯ
            var node = GetLastNode(); // ���� ������(�������) ��� ����akwlakr����

            // LastNode�� �޾ƿ� node�� �����Ѵٸ� style�� ����� ���ٸ� ���� curStyle �� �����.
            PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
            AllHideSubPopup(true);
            CurStyle = style;

            if (style == PopupStyle.None)
                return NonePopupObservable<T>();

            AddFlow(style, data); // Flow �� �߰���
            return PopupManager.Instance.Show<T>(style, data);
        }

        #endregion

        private IObservable<T> NonePopupObservable<T>() where T : PopupBase
        {
            var observer = new Subject<T>(); // Subject�� ���ø� ���·� ������ observer ����
            Observable.NextFrame().Subscribe(_ =>
            {
                observer.OnNext(null);
                observer.OnCompleted();
            }).AddTo(gameObject);
            return observer;
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
            int index = _listNode.FindIndex(node => node.Style.Equals(nextNode.Style)); // FindIndex �Լ��� ���� : startindex, count ,predicatd 

            if(index != 1) // �ߺ��� ��尡 ���� ��� �� ������ ���ķ� ����
            {
                _listNode.RemoveRange(index, (_listNode.Count - index));
            }
            _listNode.Add(nextNode);
        }

        // = SubClass 
        public class FlowNode
        {
            public PopupStyle Style { get; private set; } // ��Ÿ���� �������°� �����, �ٲ����� ���ϰ�
            public object[] Data { get; set; }

            public FlowNode(PopupStyle style, params object[] data)
            {
                Style = style;
                if (data != null && data.Length > 0) // object�� data �� �������� ����
                {
                    if (data[0] != null)
                    {
                        Data = null; // �Ķ����data�� ���� ���ٸ� �������� Data�� null�� ������
                    }
                }
            }
        }
    }
}