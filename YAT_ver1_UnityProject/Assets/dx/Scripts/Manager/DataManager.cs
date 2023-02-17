using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
using System.Security.Policy;
using Assets.Scripts.Common.DI;

namespace Assets.Scripts.Manager
{
    public class DataManager : UnitySingleton<DataManager>
    {
        public override void Initialize()
        {
            DependuncyInjection.Inject(this);
            base.Initialize();
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        //public int _loadCount;
        public GameObject _nullObject;
        public GameObject _callPrefab;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("로드카운트 변경");
                _nullObject = _callPrefab;  
            }
        }

        #region ::::: SetDatas

        #endregion
    }
}