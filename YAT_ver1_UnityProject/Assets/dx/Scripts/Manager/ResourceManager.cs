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
        // �� �Ŵ����� ����
        //[DependuncyInjection(typeof()]

        private Dictionary<string, Sprite> _imageCaches = new Dictionary<string, Sprite>();
        private Dictionary<string, GameObject> _modelCaches = new Dictionary<string, GameObject>(); // �� ĳ�� ���� ã�ƾ���

        public override void Initialize()
        {
            base.Initialize();
            //DependuncyInjection.Inject(this); // �̰� ���� ���� ��������?
        }

        #region ###������ �ε�

        // ModelPrefab �ʱ�ȭ
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
        /// <param name="path">���ҽ� ���</param>
        /// <param name="parent">�θ�ü�� TF</param>
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


        #region ###�̹��� �ε�
        public static Sprite GetImages(string id) // �̹����� �ν��Ͻ��� Ű ������ ������
        {
            if (!Instance._imageCaches.ContainsKey(id))
            {
                Debug.LogError("GetImages Error! Ű�� ã�� �� ���� Key  = " + id);
                return null;
            }
            return Instance._imageCaches[id];
        }

        public static Sprite GetPathImage(string subPath, string name) // �̹����� Resources ��η� ������
        {
            if(Instance._imageCaches.ContainsKey(name) == false) // �̹��� ĳ�ÿ� �̸��� ���� ��Ȳ
            {
                var path = string.Format("{0}{1}", subPath, name);
                var sprite = Resources.Load<Sprite>(path);
                Instance._imageCaches.Add(name, sprite); // �̹���ĳ�ÿ� ������Ų �̹����� �̸����� ĳ�� �߰���
                return sprite;
            }
            return Instance._imageCaches[name];
        }
        #endregion
    }
}