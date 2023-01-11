using System.Security.Policy;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.Common
{

    public interface ISingleton // ISingleton�� �޴� ��� ��ü���� ��� �ɹ� �Ҵ��������
    {
        void Initialize();  
        void UnInitialize();
    }

    public static class UnitySingletonObject
    {
        private static GameObject _singletonObject;
        public static Transform GetTrnasfom() // �̱��� ������Ʈ�� tf ��ȯ
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

                lock(_lock) // �̰� ���������ϱ� // Object()
                {
                    return _instance ?? (_instance = CreateInstance_ofT()); // ?? ������ : ?? ���� ���� null �̸� �ڿ��ִ� ��, null �� �ƴ϶�� �տ��ִ� ���� �����Ѵ�.
                }
            }
        }
        private static T CreateInstance_ofT() // ���ø� ���·� �ν��Ͻ� ����?  ��� �Ǵ°��� �̰�
        {
            var instnace = (T)FindObjectOfType(typeof(T));

            if (instnace != null)
                return instnace;

            var singleton = new GameObject(typeof(T).ToString()); // T ������ tostring�� ����
            singleton.transform.SetParent(UnitySingletonObject.GetTrnasfom()); // ���� ������Ų singleton ������Ʈ�� �θ�� _singletonObject �Ҵ�
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
                return CreateInstance<T>(); // ScriptableObject �����Լ�
                // ��� : 

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