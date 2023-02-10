using System;

namespace Assets.Scripts.Common.DI
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DependuncyInjectionAttribute : Attribute
	{
		public Type ObjectType;
		public DependuncyInjectionAttribute(Type objectType)
		{
			ObjectType = objectType;
		}
	}
}