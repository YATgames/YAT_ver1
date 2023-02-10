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
        private static T _instance; // 스테틱 인스턴스
        private static bool _applicationIsQuitting = false; // 애플리케이션이 종료되었는지 여부
        private static object _lock = new object();

        public static T Instance // 인스턴스 프로퍼티로 설정하기
        {
            get
            {
                if (_applicationIsQuitting)
                    return null;

                lock (_lock)
                    return _instance ?? (_instance = CreateInstanceOfT());
            }
        }
        private static T CreateInstanceOfT() // <T> 의 형태로 인스턴스를 생성한다.
        {
            var instance = (T)FindObjectOfType(typeof(T));

            if (instance != null)
                return instance;

            var singleton = new GameObject(typeof(T).ToString()); // <T> 의 타입이름으로 생성함?
            singleton.transform.SetParent(UnitySingletonObject.GetTransform()); 
            instance = singleton.AddComponent<T>();
            return instance;

        }

        // ISingleton을 상속받아서 아래 두개의 함수가 정의되어 있어야함
        public virtual void Initialize()
        {

        }

        public virtual void UnInitialize()
        {

        }

        void OnDestroy() // 애플리케이션 종료시킬때 같이 실행시켜부려
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
