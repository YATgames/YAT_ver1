using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Util
{
	[RequireComponent(typeof(ScrollRect))]
	public class ReuseComponent : MonoBehaviour
	{

		[SerializeField]
		private Component _autoCreateComponent;
		[SerializeField]
		private List<Component> _preloadComponent = new List<Component>();
		[SerializeField]
		private Transform _componentParent;

		//		[SerializeField]
		//		private bool 

		public int UpdateStartPosition = 0;
		public int AutoCreateCount = 3;
		public UpdateItem UpdateItem = delegate { };
		public OnEventTrigger FirstLoadComplete = new OnEventTrigger();

		public Component Prefix;
		public Component Suffix;

		public bool IsFirstActivePrefix = true;
		public bool IsFirstActiveSuffix = false;

		public bool IsSuffixView { get; set; }

		public Transform ComponentParent
		{
			get
			{
				if (_componentParent == null)
				{
					_componentParent = _scrollRect.content.transform;
				}
				return _componentParent;
			}
		}

		private readonly List<RectTransform> _itemList = new List<RectTransform>();
		private ScrollRect _scrollRect;
		public ScrollRect ScrollRect { get { return _scrollRect; } }
		public int Count { get; private set; }
		public int MaxViewItemCount { get; set; }
		public Vector2 OnePointSize { get; set; }

		private int _positionValue;
		private Vector2 _prefixSize;
		private Vector2 _suffixSize;
		private Vector2 _componentSize;

		private int _positionIndex;
		private bool _isCreateComponent = true;

		public bool IsCanvasSize = false;

		public ReactiveProperty<bool> IsCreated = new ReactiveProperty<bool>();
		private int _capturedCount;
		private IDisposable _createdDisposeable;
		private bool _isFirstLoadComplete;

		public int PositionIndex
		{
			get
			{
				return _positionIndex;
			}
			set
			{
				value = Mathf.Clamp(value, 0, Mathf.Max(0, Count - 1));

				if (_positionIndex.Equals(value))
					return;

				if (Prefix != null)
				{
					Prefix.gameObject.SetActive(value == 0);
				}

				if (Suffix != null)
				{
					Suffix.gameObject.SetActive(IsSuffixView && value > Count - AutoCreateCount);
				}

				_positionIndex = value;
				ComponentUpdate(false);
			}
		}

		public Component AutoCreateComponent
		{
			get { return _autoCreateComponent; }
		}

		public Rect ComponentRect { get; private set; }

		void Start()
		{
			_scrollRect = GetComponent<ScrollRect>();
			_positionValue = _scrollRect.horizontal ? 0 : 1;
			_scrollRect.onValueChanged.AsObservable().Subscribe(_ => CheckStartIndex()).AddTo(gameObject);

			UpdateItem += UpdateItemCheck;

			if (_preloadComponent.Count > 0)
			{
				AutoCreateCount = _preloadComponent.Count;
				_isCreateComponent = false;
			}

			CreateComponent();
			IsCreated.Value = true;
		}

		//void OnEnable()
		//{
		//	if (IsCreated.Value)
		//		return;
		//	_scrollRect = GetComponent<ScrollRect>();
		//	_positionValue = _scrollRect.horizontal ? 0 : 1;
		//	_scrollRect.onValueChanged.AsObservable().Subscribe(_ => CheckStartIndex()).AddTo(gameObject);
		//	CreateComponent();
		//	IsCreated.Value = true;
		//}

		private void CheckStartIndex()
		{
			PositionIndex = GetIndexByPosition(_scrollRect.content.anchoredPosition[_positionValue]);
		}

		private void ComponentUpdate(bool isCountUpdate)
		{
			if (_itemList.Count != AutoCreateCount)
				return;

			var startPositionIndex = PositionIndex + UpdateStartPosition;

			for (var i = 0; i < AutoCreateCount; i++)
			{
				var itemIndex = (i + startPositionIndex % AutoCreateCount + AutoCreateCount) % AutoCreateCount;
				var itemRectTrans = _itemList[itemIndex];

				if (itemIndex >= Count)
				{
					itemRectTrans.gameObject.SetActive(false);
				}
				else
				{
					var itemPosition = startPositionIndex + i;
					if (itemPosition >= Count || itemPosition < 0)
					{
						itemRectTrans.gameObject.SetActive(false);
					}
					else
					{
						var newPosition = Vector3.zero;

						newPosition[_positionValue] = itemPosition * _componentSize[_positionValue] * (_positionValue.Equals(0) ? 1 : -1);

						if (Prefix != null)
						{
							newPosition[_positionValue] -= _prefixSize[_positionValue];
						}

						var isUpdate = isCountUpdate || itemRectTrans.anchoredPosition3D != newPosition || !itemRectTrans.gameObject.activeSelf;

						itemRectTrans.gameObject.SetActive(true);

						if (isUpdate)
						{
							itemRectTrans.anchoredPosition3D = newPosition;
							if (UpdateItem != null)
								UpdateItem(itemPosition, itemRectTrans.gameObject);
						}
					}
				}
			}
		}

		public T GetContext<T>(int index) where T : class
		{
			if (_itemList.Count == 0 || index < -1 || AutoCreateCount.Equals(0))
				return null;

			var item = _itemList[index % AutoCreateCount];
			return item.GetComponent<T>();
		}
        private int GetIndexByPosition(float position)
		{
			position = _positionValue.Equals(0) ? -position : position;

			if (_positionValue.Equals(0))
			{
				if (Prefix != null)
				{
					position += _prefixSize[_positionValue];
				}
			}
			else
			{
				if (Prefix != null)
				{
					position -= _prefixSize[_positionValue];
				}
			}

			var index = (int)Math.Floor(position / OnePointSize[_positionValue]);
			if (index + MaxViewItemCount > Count)
			{
				index = Count - MaxViewItemCount;
			}
			if (index < 0)
			{
				index = 0;
			}
			return index;
		}

		private void CreateComponent()
		{
			if (AutoCreateComponent == null)
				return;

			if (IsCanvasSize)
			{
				var canvas = GetComponentInParent<Canvas>();
				var canvasRect = canvas.GetComponent<RectTransform>();
				ComponentRect = canvasRect.rect;
			}
			else
			{
				ComponentRect = AutoCreateComponent.GetComponent<RectTransform>().rect;
				AutoCreateComponent.gameObject.SetActive(false);
			}

			if (Prefix != null)
			{
				_prefixSize = Prefix.GetComponent<RectTransform>().rect.size;
				Prefix.transform.SetParent(ComponentParent);
				Prefix.transform.localScale = Vector3.one;
				Prefix.transform.localPosition = Vector3.zero;
				Prefix.gameObject.SetActive(IsFirstActivePrefix);
			}

			if (Suffix != null)
			{
				_suffixSize = Suffix.GetComponent<RectTransform>().rect.size;
				Suffix.transform.SetParent(ComponentParent);
				Suffix.transform.localScale = Vector3.one;
				Suffix.transform.localPosition = Vector3.zero;
				Suffix.gameObject.SetActive(IsFirstActiveSuffix);
			}

			_componentSize = ComponentRect.size.x.Equals(0) || ComponentRect.size.y.Equals(0) ? _scrollRect.viewport.rect.size : ComponentRect.size;
			for (var i = 0; i < AutoCreateCount; i++)
			{
				var item = _isCreateComponent ? Instantiate(AutoCreateComponent) : _preloadComponent[i];
				if (item == null)
				{
					Debug.LogWarning(string.Format("Invalid ReuseComponent [INDEX : {0}]", i));
					continue;
				}

				item.name = string.Format("{0}_{1}", _isCreateComponent ? AutoCreateComponent.name : item.name, i);
				item.transform.SetParent(ComponentParent);
				item.gameObject.SetActive(false);
				var rectTrans = item.GetComponent<RectTransform>();
				rectTrans.anchorMin = new Vector2(0, 1);
				rectTrans.anchorMax = new Vector2(0, 1);
				rectTrans.pivot = new Vector2(0, 1);

				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.zero;

				if (_isCreateComponent)
				{
					rectTrans.sizeDelta = _componentSize;
				}
				else
				{
					// TODO : [RJ]마지막에 잘못 된 사이즈 Component가 설정되어 있으면..?
					rectTrans.sizeDelta = _componentSize = rectTrans.rect.size.x.Equals(0) || rectTrans.sizeDelta.y.Equals(0) ? _scrollRect.viewport.rect.size : rectTrans.rect.size;
				}

				_itemList.Add(rectTrans);
			}
		}

		public void SetCount(int count)
		{
			if (Prefix != null)
				Prefix.gameObject.SetActive(true);

			_capturedCount = count;

			if (IsCreated.Value == false)
			{
				if (_createdDisposeable == null)
				{
					_createdDisposeable = IsCreated.Where(x => x).Take(1).Subscribe(_ => OnSetCount(_capturedCount)).AddTo(gameObject);
				}
				return;
			}

			Observable.NextFrame().Subscribe(_ => OnSetCount(_capturedCount)).AddTo(gameObject);
		}

		public void Clear()
		{
			_capturedCount = 0;
			OnSetCount(0);
		}

		private void OnSetCount(int count)
		{
			if (_scrollRect == null)
				return;

			_isFirstLoadComplete = false;
			if (count == 0)
			{
				_scrollRect.content.sizeDelta = _componentSize;
				OnePointSize = _scrollRect.content.rect.size;
				MaxViewItemCount = 1;
				if (Suffix != null)
				{
					Suffix.gameObject.SetActive(false);
				}
			}
			else
			{
				var sizeDelta = new Vector2(_componentSize.x * (_positionValue.Equals(0) ? count : 1), _componentSize.y * (_positionValue.Equals(0) ? 1 : count));
				OnePointSize = sizeDelta / count;

				if (Prefix != null)
				{
					sizeDelta[_positionValue] += _prefixSize[_positionValue];
				}
				if (Suffix != null)
				{
					if (IsSuffixView)
					{
						var newPosition = Vector3.zero;
						newPosition[_positionValue] = -sizeDelta[_positionValue];
						Suffix.transform.localPosition = newPosition;
						sizeDelta[_positionValue] += _suffixSize[_positionValue];
					}
					Suffix.gameObject.SetActive(false);
				}

				_scrollRect.content.sizeDelta = sizeDelta;

				MaxViewItemCount = (int)Math.Ceiling(_scrollRect.viewport.rect.size[_positionValue] / OnePointSize[_positionValue]);
			}

			Count = count;
			ComponentUpdate(true);
		}

		private void UpdateItemCheck(int index, GameObject go)
		{
			if (_isFirstLoadComplete)
				return;
			_isFirstLoadComplete = true;
			FirstLoadComplete.Invoke();
		}

		public void ResetPosition()
		{
			if (ScrollRect == null)
				return;
			if (_positionValue == 0)
			{
				ScrollRect.horizontalNormalizedPosition = 0;
			}
			else
			{
				ScrollRect.verticalNormalizedPosition = 1;
			}
		}
	}
	public delegate void UpdateItem(int index, GameObject go);
}
