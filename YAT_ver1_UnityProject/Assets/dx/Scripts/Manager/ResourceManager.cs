using Assets.Scripts.Common;
using Assets.Scripts.Common.DI;
using Assets.Scripts.PrefabModel;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ResourcesManager : UnitySingleton<ResourcesManager>
    {
        // 각 매니저들 생성
        //[DependuncyInjection(typeof()]

        private Dictionary<string, Sprite> _imageCaches = new Dictionary<string, Sprite>();
        private Dictionary<string, GameObject> _modelCaches = new Dictionary<string, GameObject>(); // 모델 캐시 사용법 찾아야함

        public override void Initialize()
        {
            base.Initialize();
            //DependuncyInjection.Inject(this); // 이거 순서 영향 있으려나?
        }

        #region ###프리팹 로드

        // ModelPrefab 초기화
        public static void ResetModels(string id)
        {
            if(Instance._modelCaches.ContainsKey(id))
            {
                var parent = Instance._modelCaches[id].GetComponent<ModelPrefabBase>().Parent;
                Instance._modelCaches[id].transform.SetParent(parent);
                Instance._modelCaches[id].gameObject.SetActive(false);
            }
        }


        /// <summary> </summary>
        /// <param name="path">리소스 경로</param>
        /// <param name="parent">부모객체의 TF</param>
        /// <returns></returns>
        public static GameObject LoadAndInit(string path, Transform parent)
        {
            var pathLoad = Load(path);
            if(pathLoad == null)
            {
                Debug.LogWarning("LoadAndInit Erorr! path = " + path);
                return null;
            }
            var item = Instantiate(pathLoad, parent);
            var transform = item.GetComponent<Transform>();

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            return item;
        }

        public static GameObject Load(string path)
        {
            return Resources.Load<GameObject>(path);
        }
        #endregion


        #region ###이미지 로드
        public static Sprite GetImages(string id) // 이미지를 인스턴스의 키 값으로 가져옴
        {
            if (!Instance._imageCaches.ContainsKey(id))
            {
                Debug.LogError("GetImages Error! 키를 찾을 수 없음 Key  = " + id);
                return null;
            }
            return Instance._imageCaches[id];
        }

        public static Sprite GetPathImage(string subPath, string name) // 이미지를 Resources 경로로 가져옴
        {
            if(Instance._imageCaches.ContainsKey(name) == false) // 이미지 캐시에 이름이 없는 상황
            {
                var path = string.Format("{0}{1}", subPath, name);
                var sprite = Resources.Load<Sprite>(path);
                Instance._imageCaches.Add(name, sprite); // 이미지캐시에 생성시킨 이미지의 이름으로 캐시 추가함
                return sprite;
            }
            return Instance._imageCaches[name];
        }
        #endregion
    }
}