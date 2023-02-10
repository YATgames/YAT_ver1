using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Common
{ 
    public interface ISingleton
    {
        void Initialize();
        void UnInitialize();
    }
    public static class UnitySingletonObject
    {
        private static GameObject _singletonObjet;
        public static Transform GetTransform()
        {
            if(_singletonObjet == null)
            {
                const string objectName = "UnitySingletons";
                _singletonObjet = ObjectFinder.Find(objectName) ?? new GameObject { name = objectName };
                UnityEngine.Object.DontDestroyOnLoad(_singletonObjet);
            }
            return _singletonObjet.transform;
        }
    }
    public class UnitySingleton<T> : MonoBehaviour, ISingleton where T : UnitySingleton<T>
    {
        private static T _instance; // ����ƽ �ν��Ͻ�
        private static bool _applicationIsQuitting = false; // ���ø����̼��� ����Ǿ����� ����
        private static object _lock = new object();

        public static T Instance // �ν��Ͻ� ������Ƽ�� �����ϱ�
        {
            get
            {
                if (_applicationIsQuitting)
                    return null;

                lock (_lock)
                    return _instance ?? (_instance = CreateInstanceOfT());
            }
        }
        private static T CreateInstanceOfT() // <T> �� ���·� �ν��Ͻ��� �����Ѵ�.
        {
            var instance = (T)FindObjectOfType(typeof(T));

            if (instance != null)
                return instance;

            var singleton = new GameObject(typeof(T).ToString()); // <T> �� Ÿ���̸����� ������?
            singleton.transform.SetParent(UnitySingletonObject.GetTransform()); 
            instance = singleton.AddComponent<T>();
            return instance;

        }

        // ISingleton�� ��ӹ޾Ƽ� �Ʒ� �ΰ��� �Լ��� ���ǵǾ� �־����
        public virtual void Initialize()
        {

        }

        public virtual void UnInitialize()
        {

        }

        void OnDestroy() // ���ø����̼� �����ų�� ���� ������Ѻη�
        {
            _applicationIsQuitting = true;
        }





        public class Singleton<T> : ScriptableObject, ISingleton where T : Singleton<T>
        {
            private static T _instance = null;
            private static object _lock = new object();

            public static T Instance
            {
                get
                {
                    lock (_lock)
                    {
                        return _instance ?? (_instance = CreateInstanceOfT());
                    }
                }
            }
            private static T CreateInstanceOfT()
            {
                var path = string.Format("DataBundles/{0}", typeof(T).Name);
                try
                {
                    var data = Resources.Load<T>(path);
                    return data;
                }
                catch
                {
                    return CreateInstance<T>();
                }
            }
            public virtual void Initialize()
            {

            }
            public virtual void UnInitialize()
            {

            }
        }
    }

}
