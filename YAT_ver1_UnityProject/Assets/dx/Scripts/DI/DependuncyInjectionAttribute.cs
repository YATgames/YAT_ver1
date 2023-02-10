using System;

namespace Assets.Scripts.Common.DI // DependuncyInjection 의 약자
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DependuncyInjectionAttribute : Attribute // 의존성 부여하기 
    {
        public Type ObjectType;
        public DependuncyInjectionAttribute(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}