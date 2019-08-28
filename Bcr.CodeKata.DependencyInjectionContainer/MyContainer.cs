using System;
using System.Collections.Generic;

namespace Bcr.CodeKata.DependencyInjectionContainer
{
    public class MyContainer
    {
        private Dictionary<Type, Type> typeDictionary = new Dictionary<Type, Type>();

        public void Register<T1, T2>()
        {
            typeDictionary[typeof(T1)] = typeof(T2);
        }

        public T Resolve<T>()
        {
            return (T) typeDictionary[typeof(T)].GetConstructor(new Type[0]).Invoke(null);
        }
    }
}
