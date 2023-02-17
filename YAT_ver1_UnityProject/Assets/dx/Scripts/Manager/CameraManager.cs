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
                    // OrderByDescending : ����Ʈ �����ϱ�(ī�޶��� depth �������)
                    _cameras.OrderByDescending(camera1 => camera1.depth).First()  // _cameras ����Ʈ�� 0 �̻��� ��
                    : null; }  // ���� ��
        }
        private readonly List<Camera> _cameras = new List<Camera>();
        private IDisposable _disposable; // �������� ���� ���ҽ� ������ ���� ��Ŀ���� ����

        public void SetMainCamera(Camera value)
        {

            if(Camera != null) // ������ �����Ǿ��ִ� ī�޶� �־��ٸ� ����� ���� �����Ѵ�
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
            if (Camera == null) // ������ ī�޶� ���ٸ� �� �ʿ� ���� ����
                return;
            Debug.Log("ChangeResolution ����");
            //Camera.transform.localPosition= new Vector3()
           
        }

        public override void Initialize()
        {
            base.Initialize();

            SetMainCamera(ObjectFinder.Find("Main Camera").GetComponent<Camera>()); // ���� ī�޶� ����
        }
        public override void UnInitialize()
        {
            base.UnInitialize();

            foreach (var camera1 in _cameras)   
            {
                DestroyCamera(camera1);
            }
            _cameras.Clear();
            if (_disposable == null) // ��Ȱ��ȭ �ϴµ� ���ִ� ��ü�� ���ٸ� ������������
                return;
            _disposable.Dispose();
            _disposable = null;
        }
    }
}