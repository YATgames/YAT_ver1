using System;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class PopupBase : MonoBehaviour
	{
		public bool DontDestroy;
		private PopupStyle _popupStyle;
		public PopupStyle PopupStyle
		{
			get
			{
				if (!_popupStyle.Equals(PopupStyle.None))
					return _popupStyle;

				var popupName = gameObject.name;
				popupName = popupName.Replace("UIPopup", "");
				popupName = popupName.Replace("(Clone)", "");
				try
				{
					_popupStyle = (PopupStyle)Enum.Parse(typeof(PopupStyle), popupName);
				}
				catch (Exception)
				{
					_popupStyle = PopupStyle.None;
				}
				return _popupStyle;
			}
		}
		//target camera가 바뀌면서 disable 되더라도 raycast가 살아있어 다른 view의 event가 발생하지 않는 문제때문에 추가됨.
		private bool _onRaycast;
		// = Field ================================================================================
		protected GameObject Camera;  // 메인 카메라
		protected CompositeDisposable CancelerObject;

		public bool IsActive
		{
			get { return gameObject.activeInHierarchy; }
		}

		public bool IsIgnoreEscapeHide { get; set; }

		// = Construct ============================================================================
		void Awake()
		{
			Initialize();
		}

		public virtual void Initialize()
		{

		}

		protected virtual void UnInitialize()
		{

		}

		protected virtual void LoadComplete()
		{
			PopupManager.Instance.LoadCompletePopup.Invoke(this);
		}

		//protected GameObject Cancelor;

		// = Get / Set ============================================================================
		/// <summary>
		/// 화면을 출력
		/// </summary>
		/// <param name="data"></param>
		public virtual void Show(params object[] data)
		{
			if (CancelerObject != null)
				CancelerObject.Dispose();

			CancelerObject = new CompositeDisposable();
			gameObject.SetActive(true);
		}


		/// <summary>
		/// 닫기
		/// </summary>
		public virtual void Hide()
		{
			if (CancelerObject != null)
			{
				CancelerObject.Dispose();
				CancelerObject = null;
			}

			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			if (DontDestroy)
				return;

			Destory();
		}

		private void Destory()
		{
			if (PopupManager.Instance)
				PopupManager.Instance.PopupList.Remove(this);
			Destroy(gameObject);
		}

		protected virtual void OnDestroy()
		{
			Hide();
			UnInitialize();
			Resources.UnloadUnusedAssets();
		}

		//parent가 camera를 따로 가지고 있다면 renderMode및 worldCamera를 교체해주기 위한 메소드 ( raycast에대한 enabled속성도 이곳에서 설정한다. )
		public void SetParent(Transform parent)
		{
			transform.SetParent(parent);
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;

            //var cameraComponent = parent.GetComponentInChildren<Camera>();
            //if (cameraComponent == null)
            //{
            //    return;
            //}

            //var canvasComponent = GetComponentInChildren<Canvas>();
            //canvasComponent.renderMode = RenderMode.WorldSpace;
            //canvasComponent.worldCamera = cameraComponent;
            //canvasComponent.planeDistance = 600;
            //var rectTransform = canvasComponent.GetComponent<RectTransform>();
            //rectTransform.pivot = Vector2.zero;
            //rectTransform.localPosition = new Vector3(Constants.ScreenSize.x / -2, Constants.ScreenSize.y / -2, 1);
            //rectTransform.localScale = Vector3.one;
            //rectTransform.sizeDelta = Constants.ScreenSize;
            //var rayCast = canvasComponent.GetComponent<GraphicRaycaster>();

            //this.UpdateAsObservable().Subscribe(_ =>
            //{
            //    if (!_onRaycast.Equals(cameraComponent.enabled))
            //    {
            //        _onRaycast = rayCast.enabled = cameraComponent.enabled;
            //    }
            //}).AddTo(gameObject);
        }

	}
}