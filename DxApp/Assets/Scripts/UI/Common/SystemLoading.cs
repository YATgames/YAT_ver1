using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Common
{
	public class SystemLoading : MonoBehaviour
	{
		public enum LoadingSize
		{
			Big,
			Small,
		}
		private const string _path = "Prefs/SystemLoading/{0}";
		private static List<SystemLoading> _lodingList = new List<SystemLoading>();

		private MonoBehaviour _parent;

		public static void Hide<T>(T root) where T : MonoBehaviour
		{
			var item = _lodingList.FirstOrDefault(v => v._parent.Equals(root));
			if (item != null)
			{
				_lodingList.Remove(item);
				Destroy(item.gameObject);
			}
		}

		public static void Show<T>(LoadingSize size, T root) where T : MonoBehaviour
		{
			if (_lodingList.Where(v => v._parent.Equals(root)).Count() > 0)
				return;

			var instance = Instantiate(Resources.Load<SystemLoading>(string.Format(_path, size)));
			instance._parent = root;
			if (size == LoadingSize.Small)
			{
				instance.transform.SetParent(root.transform);
			}
			else
			{
				instance.transform.SetParent(PopupManager.Instance.transform);
			}

			instance.transform.SetAsLastSibling();
			var rect = instance.GetComponent<RectTransform>();

			_lodingList.Add(instance);
		}
	}
}
