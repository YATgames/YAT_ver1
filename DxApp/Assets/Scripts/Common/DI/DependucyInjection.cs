using System;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Common.DI
{
	public class DependuncyInjection
	{
		private static readonly Dictionary<Type, ISingleton> CachedSingletons = new Dictionary<Type, ISingleton>();
		public static void Inject(Object unityObject)
		{
			var t = unityObject.GetType();
			var infos = t.GetFields(BindingFlags.Instance |
									BindingFlags.NonPublic |
									BindingFlags.Public);

			foreach (var fi in infos)
			{
				if (false == typeof(ISingleton).IsAssignableFrom(fi.FieldType) ||
					false == fi.IsDefined(typeof(DependuncyInjectionAttribute), true))
				{
					continue;
				}

				var attr = Attribute.GetCustomAttributes(fi)[0];
				var dependuncyInjectionAttribute = attr as DependuncyInjectionAttribute;
				var dependuncyInjectionObjectType = dependuncyInjectionAttribute.ObjectType;
				ISingleton found = null;

				if (CachedSingletons.TryGetValue(dependuncyInjectionObjectType, out found))
				{
					fi.SetValue(unityObject, found);
					continue;
				}
				found = (ISingleton)Object.FindObjectOfType(dependuncyInjectionObjectType) ??
						(ISingleton)dependuncyInjectionObjectType.GetProperty("Instance",
							BindingFlags.Static |
							BindingFlags.Public |
							BindingFlags.FlattenHierarchy |
							BindingFlags.GetProperty).GetValue(null, null);

				fi.SetValue(unityObject, found);
				CachedSingletons.Add(dependuncyInjectionAttribute.ObjectType, found);
			}
		}
	}
}