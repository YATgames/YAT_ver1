using System;

namespace Assets.Scripts.Common.DI // DependuncyInjection �� ����
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DependuncyInjectionAttribute : Attribute // ������ �ο��ϱ� 
    {
        public Type ObjectType;
        public DependuncyInjectionAttribute(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}