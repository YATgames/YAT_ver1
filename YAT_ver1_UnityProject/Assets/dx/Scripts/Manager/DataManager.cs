using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Util;
namespace Assets.Scripts.Manager
{
    public class DataManager : UnitySingleton<DataManager>
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
        }
        public int _loadCount;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("�ε�ī��Ʈ ����");
                _loadCount++;
            }
        }

        #region ::::: SetDatas

        #endregion
    }
}