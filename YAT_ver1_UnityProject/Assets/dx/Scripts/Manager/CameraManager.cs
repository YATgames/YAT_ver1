using Assets.Scripts.Common;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class CameraManager : UnitySingleton<CameraManager>
    {
        public Camera Camera { get; set; }
        public Camera LastDepthCamera 
        {
            get { return _cameras.Count > 0 ?
                    // OrderByDescending : 리스트 정렬하기(카메라의 depth 순서대로)
                    _cameras.OrderByDescending(camera1 => camera1.depth).First()  // _cameras 리스트가 0 이상일 때
                    : null; }  // 없을 때
        }
        private readonly List<Camera> _cameras = new List<Camera>();
        private IDisposable _disposable; // 관리되지 않은 리소스 해제를 위한 메커니즘 제공

        public void SetMainCamera(Camera value)
        {

            if(Camera != null) // 기존에 설정되어있는 카메라가 있었다면 지우고 새로 설정한다
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
        private void DestroyCamera(Camera value)
        {   
            DestroyImmediate(value.gameObject);
        }
        private void UpdatePosition()
        {
            if (Camera == null) // 설정된 카메라가 없다면 할 필요 없는 동작
                return;
            Debug.Log("ChangeResolution 동작");
            //Camera.transform.localPosition= new Vector3()
           
        }

        public override void Initialize()
        {
            base.Initialize();

            SetMainCamera(ObjectFinder.Find("Main Camera").GetComponent<Camera>()); // 메인 카메라 설정
        }
        public override void UnInitialize()
        {
            base.UnInitialize();

            foreach (var camera1 in _cameras)   
            {
                DestroyCamera(camera1);
            }
            _cameras.Clear();
            if (_disposable == null) // 비활성화 하는데 해주는 객체가 없다면 동작하지않음
                return;
            _disposable.Dispose();
            _disposable = null;
        }
    }
}