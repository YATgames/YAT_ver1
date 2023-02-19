using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;

using Assets.Scripts.Common.DI;
using UnityEditor.Rendering;

namespace Assets.Scripts.Manager
{
    public class DataManager : UnitySingleton<DataManager>
    {
        public override void Initialize()
        {
            base.Initialize();
            DependuncyInjection.Inject(this);
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        //public int _loadCount;
        public GameObject _nullObject;
        public GameObject _callPrefab;

        private void Awake()
        {
            _callPrefab = Resources.Load<GameObject>("Prefs/Test");
            Debug.Log(_callPrefab.name);
        }
        public void ChagneValue()
        {
            _nullObject = _callPrefab;
            if (_nullObject == null)
                Debug.Log("<color=red> null 반환함 다시해야함</color>");
            else
                Debug.Log("<color=blue> 일단 받긴함</color>");
        }
        #region ::::: SetDatas

        #endregion
    }
}