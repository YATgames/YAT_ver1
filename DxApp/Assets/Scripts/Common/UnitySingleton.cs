using UnityEngine;

namespace Assets.Scripts.Common
{
	public interface ISingleton
	{
		void Initialize();
		void UnInitialize();
	}

	public static class UnitySingletonsObject
	{
		private static GameObject _singletonObject;
		public static Transform GetTransform()
		{
			if (_singletonObject == null)
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
				if (_applicationIsQuitting)
				{
					return null;
				}

				lock (_lock)
				{
					return _instance ?? (_instance = CreateInstanceOfT()); ;
				}
			}
		}

		private static T CreateInstanceOfT()
		{
			var instance = (T)FindObjectOfType(typeof(T));

			if (instance != null)
				return instance;

			var singleton = new GameObject(typeof(T).ToString());
			singleton.transform.SetParent(UnitySingletonsObject.GetTransform());
			instance = singleton.AddComponent<T>();
			return instance;
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
