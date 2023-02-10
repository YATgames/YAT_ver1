using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
	[RequireComponent(typeof(RectTransform))]
	public class DragItem<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
	{
		private bool _isDown = false;
		private Vector3 _initPos;
		private Transform _parent;
		private RectTransform _rect;

		private GameObject _dragParent;
		protected T _this;
		public OnEventTrigger<T> OnDragItem { get; set; }
		public OnEventTrigger OnEndDragItem { get; set; }

		protected virtual void Start()
		{
			_rect = gameObject.GetComponent<RectTransform>();
			_parent = _rect.parent;
			_initPos = _rect.localPosition;
		}

		protected virtual void End()
		{
			_rect.SetParent(_parent);
			_rect.localPosition = _initPos;
			_rect.localScale = Vector3.one;

			if (_dragParent)
			{
				Destroy(_dragParent);
				OnEndDragItem?.Invoke();
			}
		}

		protected virtual void PointerDown(PointerEventData eventData)
		{
			if (_isDown) return;

			_dragParent = new GameObject();
			_dragParent.name = "DragParent";

			var canvas = _dragParent.AddComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.sortingOrder = 1000;
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = CameraManager.Instance.Camera;

			var canvasScaler = _dragParent.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.matchWidthOrHeight = 1;
			canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);

			_rect.SetParent(_dragParent.transform);
			_rect.localScale = new Vector3((float)Screen.width / 720, (float)Screen.height / 1280);

			_rect.anchorMin = Vector2.zero;
			_rect.anchorMax = Vector2.zero;
			_rect.pivot = new Vector2(.5f, .5f);
			_rect.anchoredPosition3D = eventData.position;

			_isDown = true;
			OnDragItem?.Invoke(_this);
		}

		protected virtual void PointerUp(PointerEventData eventData)
		{
			End();
			_isDown = false;
		}

		protected virtual void Drag(PointerEventData eventData)
		{
			_rect.anchoredPosition3D = eventData.position;
		}

		protected virtual void EndDrag(PointerEventData eventData)
		{
			End();
			_isDown = false;
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			PointerDown(eventData);
		}
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			PointerUp(eventData);
		}
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			Drag(eventData);
		}
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			EndDrag(eventData);
		}
	}
}
