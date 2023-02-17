using Assets.Scripts.Common;
using Assets.Scripts.UI;
using Assets.Scripts.Common.DI; // DependuncyInjection
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Assets.Scripts.Manager
{
    public class ConnectionManager : UnitySingleton<ConnectionManager>
    {
        [DependuncyInjection(typeof(FlowManager))]
        private FlowManager _flowManager;
        [DependuncyInjection(typeof(GameManager))]
        private GameManager _gameManager;
        [DependuncyInjection(typeof(ResourcesManager))]
        private ResourcesManager resourcesManager;



        public override void Initialize()
        {
            base.Initialize();
        }

        private T GetResult<T>(object result) // ��� �޾ƿ��¹��
        {
            return JsonConvert.DeserializeObject<T>(result.ToString());
        }


    }
}