using UnityEngine;
using UnityEngine.Rendering;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
    private static T _instance;
    public static T instance
    {
        get
        {
            CreateInstance();
            return _instance;
        }
    }

    public static void CreateInstance()
    {
        if (_instance == null)
        {
            // �����ϴ� �ν��Ͻ� ã��
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
            }
            // �ʿ��ϴٸ� �ν��Ͻ� �ʱ�ȭ
            if (!_instance.initialized)
            {
                _instance.Initialize();
                _instance.initialized = true;
            }
        }
    }

    protected bool initialized;
    protected virtual void Initialize() { }

}
