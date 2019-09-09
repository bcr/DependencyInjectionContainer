using System;
using System.Collections.Generic;
using System.Linq;

namespace Bcr.CodeKata.DependencyInjectionContainer
{
    public class MyContainer
    {
        interface ITypeEntry
        {
            object Resolve(MyContainer resolver);
        }

        class GreedyConstructorTypeEntry : ITypeEntry
        {
            public Type ObjectType { get; set; }
            public object Resolve(MyContainer resolver)
            {
                // Find all the constructors, order them by the number of
                // parameters they take in descending order (longest first)
                // and then find the first one that has all of its types
                // registered
                // ObjectType.GetConstructors()[0].GetParameters()[0].ParameterType;
                var winner = ObjectType.GetConstructors()
                    .OrderByDescending(x => x.GetParameters().Length)   // Constructors
                    .First(x => x.GetParameters()
                        .All(param => resolver.IsRegistered(param.ParameterType)));
                return winner.Invoke(winner.GetParameters().Select(x => x.ParameterType).Select(
                            type => resolver.Resolve(type)
                            ).ToArray()
                            );
            }
        }

        class SingletonTypeEntry : ITypeEntry
        {
            public object Singleton { get; set; }
            public object Resolve(MyContainer resolver)
            {
                return Singleton;
            }
        }

        private Dictionary<Type, ITypeEntry> typeDictionary = new Dictionary<Type, ITypeEntry>();

        public void Register<T1, T2>()
        {
            typeDictionary[typeof(T1)] = new GreedyConstructorTypeEntry() { ObjectType = typeof(T2) };
        }

        public object Resolve(Type T)
        {
            return typeDictionary[T].Resolve(this);
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        public void Register<T>(T singleton)
        {
            typeDictionary[typeof(T)] = new SingletonTypeEntry() { Singleton = singleton };
        }

        private bool IsRegistered(Type t)
        {
            return typeDictionary.ContainsKey(t);
        }
    }
}
