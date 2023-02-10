using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class FlowManager : UnitySingleton<FlowManager>
	{
		private readonly List<FlowNode> _listNode = new List<FlowNode>();
		public PopupStyle CurStyle { get; private set; } = PopupStyle.None;
		
		// = Construct ================================================================================

		// = Get / Set ================================================================================
		public FlowNode AddFlow(PopupStyle style, params object[] data)
		{
			Push(new FlowNode(style, data));
			//			Debug.Log(ToString());
			return GetLastNode();
		}

		public FlowNode GetLastNode()
		{
			return (_listNode.Count > 0 ? _listNode[_listNode.Count - 1] : null);
		}

		public IObservable<T> AddSubPopup<T>(PopupStyle style, params object[] data) where T : PopupSub
		{
			return PopupManager.Instance.Show<T>(style, data);
		}

		public void AddSubPopup(PopupStyle style, params object[] data)
		{
			PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
		}
		//test Contents

		public void AddContentsPopup(PopupStyle style, params object[] data)
        {
			PopupManager.Instance.Show<PopupSub>(style, data).Subscribe();
		}
		public void Change(PopupStyle style, params object[] data)
		{
			if (CurStyle == style && style != PopupStyle.None)
				return;

			var node = GetLastNode();
			PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
			AllHideSubPopup(true);
			CurStyle = style;

			if (style == PopupStyle.None)
				return;

			AddFlow(style, data);
			PopupManager.Instance.Show<PopupBase>(style, data).Subscribe();
		}

		public IObservable<T> Change<T>(PopupStyle style, params object[] data) where T : PopupBase
		{
			if (CurStyle == style && style != PopupStyle.None)
				return NonePopupObservable<T>();

			var node = GetLastNode();
			PopupManager.Instance.Hide(node != null ? node.Style : CurStyle);
			AllHideSubPopup(true);
			CurStyle = style;

			if (style == PopupStyle.None)
				return NonePopupObservable<T>();

			AddFlow(style, data);
			return PopupManager.Instance.Show<T>(style, data);
		}

		private IObservable<T> NonePopupObservable<T>() where T : PopupBase
		{
			var observer = new Subject<T>();
			Observable.NextFrame().Subscribe(_ =>
			{
				observer.OnNext(null);
				observer.OnCompleted();
			}).AddTo(gameObject);
			return observer;
		}

		private void AllHideSubPopup(bool isForce = false)
		{
			// 서브 팝업 몽땅 닫기 //
			var popupList = PopupManager.Instance.GetShowingSubPopupList();
			foreach (var subPopup in popupList)
			{
				if (isForce)
				{
					subPopup.Hide();
				}
				else
				{
					if (!subPopup.IsIgnoreEscapeHide)
					{
						subPopup.Hide();
					}
				}
			}
		}

		private void AllHidePopup()
		{
			// 팝업 몽땅 닫기 //
			if (_listNode.Count > 0)
			{
				int len = _listNode.Count;

				//				Debug.Log("::팝업 체크 로직::");
				for (var i = 0; i < len; i++)
				{
					//					Debug.Log("_listNode[" + i + "] : " + _listNode[i]);
					PopupManager.Instance.Hide(_listNode[i].Style);
				}
				//				Debug.Log("::::::::::::::::::::::::::::::");
				_listNode.Clear();
			}
		}



		public override string ToString()
		{
			var str = new StringBuilder();
			foreach (var node in _listNode)
			{
				str.Append(node.Style);
				str.Append(" / ");
			}
			return str.ToString();
		}

		// = Method ===================================================================================
		private void Push(FlowNode nextNode)
		{
			if (nextNode == null)
			{
				_listNode.Clear();
				return;
			}

			int index = _listNode.FindIndex(node => node.Style.Equals(nextNode.Style));

			if (index != -1) // 중복된 노드가 있을 경우 그 노드부터 이후로 제거한다.
			{
				_listNode.RemoveRange(index, (_listNode.Count - index));
			}
			_listNode.Add(nextNode);
		}

		// = Sub Class ================================================================================
		public class FlowNode
		{
			public PopupStyle Style { get; private set; }

			public object[] Data { get; set; }

			public FlowNode(PopupStyle style, params object[] data)
			{
				Style = style;
				if (data != null && data.Length > 0)
				{
					if (data[0] != null)
					{
						Data = data;
					}
				}
			}
		}

		private IDisposable _escapeDisposable;
		public override void Initialize()
		{
			base.Initialize();
			DependuncyInjection.Inject(this);
			_escapeDisposable = this.OnKeyDown(KeyCode.Escape).Subscribe(keyCode =>
			{
				switch (keyCode)
				{
					case KeyCode.Escape:
						// 뒤로가기 팝업 닫기 기능 구현 예정--
						break;
				}
			}).AddTo(gameObject);
		}

		public override void UnInitialize()
		{
			AllHidePopup();
			AllHideSubPopup(true);
			CurStyle = PopupStyle.None;

			if (_escapeDisposable == null) return;
			_escapeDisposable.Dispose();
			_escapeDisposable = null;
		}
	}
}
