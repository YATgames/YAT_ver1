using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CameraManager : UnitySingleton<CameraManager>
	{
		public Camera Camera { get; private set; }

		public Camera LastDepthCamera
		{
			get { return _cameras.Count > 0 ? _cameras.OrderByDescending(camera1 => camera1.depth).First() : null; }
		}

		private readonly List<Camera> _cameras = new List<Camera>();
		private IDisposable _disposable;

		public void SetMainCamera(Camera value)
		{
			if (Camera != null)
			{
				DestroyCamera(Camera);
				_cameras.Remove(Camera);
			}
			value.transform.SetParent(transform);
			Camera = value;
			UpdatePosition();
			_cameras.Add(value);
		}

		public void AddSubCamera(Camera value)
		{
			_cameras.Add(value);
		}

		public void RemoveSubCamera(Camera value)
		{
			_cameras.Remove(value);
		}

		public override void Initialize()
		{
			base.Initialize();

			SetMainCamera(ObjectFinder.Find("Main Camera").GetComponent<Camera>());
			_disposable =
				ChangeResolution.Instance.ObserveEveryValueChanged(v => v.ScreenSize)
					.Subscribe(_ => UpdatePosition())
					.AddTo(gameObject);
		}

		private void DestroyCamera(Camera value)
		{
			DestroyImmediate(value.gameObject);
		}
		public override void UnInitialize()
		{
			base.UnInitialize();

			foreach (var camera1 in _cameras)
			{
				DestroyCamera(camera1);
			}
			_cameras.Clear();
			if (_disposable == null)
				return;
			_disposable.Dispose();
			_disposable = null;
		}

		private void UpdatePosition()
		{
			if (Camera == null)
				return;
			Camera.transform.localPosition = new Vector3((int)(ChangeResolution.Instance.ScreenSize.x / 2), (int)(ChangeResolution.Instance.ScreenSize.y / 2), 0);
            var screenAspectRatio = ChangeResolution.Instance.ScreenSize.x / ChangeResolution.Instance.ScreenSize.y;
            Camera.fieldOfView = screenAspectRatio > 1.5f ? 149 : 156;
        }
	}
}
