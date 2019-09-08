using System;
using System.Collections.Generic;

namespace Bcr.CodeKata.DependencyInjectionContainer
{
    public class MyContainer
    {
        private Dictionary<Type, Type> typeDictionary = new Dictionary<Type, Type>();
        private Dictionary<Type, Object> singletonDictionary = new Dictionary<Type, Object>();

        public void Register<T1, T2>()
        {
            typeDictionary[typeof(T1)] = typeof(T2);
        }

        public T Resolve<T>()
        {
            if (singletonDictionary.ContainsKey(typeof(T)))
            {
                return (T) singletonDictionary[typeof(T)];
            }
            else
            {
                return (T) typeDictionary[typeof(T)].GetConstructor(new Type[0]).Invoke(null);
            }
        }

        public void Register<T>(T singleton)
        {
            singletonDictionary[typeof(T)] = singleton;
        }
    }
}
