using System.Security.Policy;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.Common
{

    public interface ISingleton // ISingleton을 받는 모든 객체에서 모든 맴버 할당해줘야하
    {
        void Initialize();  
        void UnInitialize();
    }

    public static class UnitySingletonObject
    {
        private static GameObject _singletonObject;
        public static Transform GetTrnasfom() // 싱글톤 오브젝트의 tf 반환
        {
            if(_singletonObject == null)
            {
                const string objectName = "UnitySingletons";
                _singletonObject = ObjectFinder.Find(objectName) ?? new GameObject { name = objectName };
                UnityEngine.Object.DontDestroyOnLoad(_singletonObject);
            }
            return _singletonObject.transform;
        }
    }

    public class UnitySingleton<T> : MonoBehaviour, ISingleton where T : UnitySingleton<T> 
    {
        private static T _instance;
        private static bool _applicationIsQuitting = false;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if(_applicationIsQuitting)
                {
                    return null;
                }

                lock(_lock) // 이게 무슨문법일까 // Object()
                {
                    return _instance ?? (_instance = CreateInstance_ofT()); // ?? 연산자 : ?? 앞의 값이 null 이면 뒤에있는 값, null 이 아니라면 앞에있는 값을 대입한다.
                }
            }
        }
        private static T CreateInstance_ofT() // 탬플릿 형태로 인스턴스 생성?  어떻게 되는거지 이건
        {
            var instnace = (T)FindObjectOfType(typeof(T));

            if (instnace != null)
                return instnace;

            var singleton = new GameObject(typeof(T).ToString()); // T 형태의 tostring이 뭐냥
            singleton.transform.SetParent(UnitySingletonObject.GetTrnasfom()); // 새로 생성시킨 singleton 오브젝트의 부모로 _singletonObject 할당
            instnace = singleton.AddComponent<T>();

            return instnace;
        }

        public virtual void Initialize()
        {

        }
        public virtual void UnInitialize()
        {

        }
        void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
    public class Singleton<T> : ScriptableObject, ISingleton where T : Singleton<T>
    {
        private static T _instance = null;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
               lock(_lock)
                {
                    return _instance ?? (_instance = CreateInstance_ofT());
                }
            }
        }
        private static T CreateInstance_ofT()
        {
            var path = string.Format("StringType/{0}", typeof(T).Name);
            try
            {
                var data = Resources.Load<T>(path);
                return data;
            }
            catch
            {
                return CreateInstance<T>(); // ScriptableObject 내장함수
                // 기능 : 

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