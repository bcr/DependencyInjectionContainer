using System;
using Xunit;

namespace Bcr.CodeKata.DependencyInjectionContainer.Test
{
    interface IFoo
    {
    }

    class Foo : IFoo
    {
        public Foo()
        {
        }

        public Foo(string param) => MyString = param;
        public Foo(string param, double param2, int param3)
        {
            MyString = param;
            MyDouble = param2;
            MyInt = param3;
        }

        public string MyString { get; private set; } = "";
        public double MyDouble { get; private set; } = 0.0;
        public int MyInt { get; private set; } = 0;
    }

    public class MyContainerTest
    {
        [Fact]
        public void container_should_be_creatable()
        {
            var container = new MyContainer();
            Assert.NotNull(container);
        }

        [Fact]
        public void resolve_returns_instance_of_registered_type()
        {
            var container = new MyContainer();

            container.Register<IFoo, Foo>();

            IFoo result = container.Resolve<IFoo>();

            Assert.IsAssignableFrom<IFoo>(result);
        }

        [Fact]
        public void multiple_resolve_returns_differnt_instance_of_registered_type()
        {
            var container = new MyContainer();

            container.Register<IFoo, Foo>();

            IFoo result = container.Resolve<IFoo>();
            IFoo result2 = container.Resolve<IFoo>();

            Assert.NotSame(result, result2);
        }

        [Fact]
        public void resolve_returns_singleton()
        {
            var container = new MyContainer();
            var singleton = new Foo();

            container.Register<IFoo>(singleton);

            IFoo result = container.Resolve<IFoo>();
            IFoo result2 = container.Resolve<IFoo>();

            Assert.Same(singleton, result);
            Assert.Same(result, result2);
        }

        [Fact]
        public void resolve_with_constructor_injection()
        {
            var container = new MyContainer();

            container.Register<IFoo, Foo, string>();
            container.Register<string>("My single string");

            IFoo result = container.Resolve<IFoo>();
            IFoo result2 = container.Resolve<IFoo>();

            Assert.NotSame(result, result2);
            Assert.Equal("My single string", ((Foo) result).MyString);
        }

        [Fact]
        public void resolve_with_constructor_injection_changing_registration_order()
        {
            MyContainer container;

            {
                container = new MyContainer();

                container.Register<string>("My single string");
                container.Register<IFoo, Foo, string>();

                IFoo result = container.Resolve<IFoo>();
                IFoo result2 = container.Resolve<IFoo>();

                Assert.NotSame(result, result2);
                Assert.Equal("My single string", ((Foo) result).MyString);
            }

            {
                container = new MyContainer();

                container.Register<IFoo, Foo, string>();
                container.Register<string>("My single string");

                IFoo result = container.Resolve<IFoo>();
                IFoo result2 = container.Resolve<IFoo>();

                Assert.NotSame(result, result2);
                Assert.Equal("My single string", ((Foo) result).MyString);
            }
        }

        [Fact]
        public void resolve_with_greedy_constructor_match()
        {
            var container = new MyContainer();

            // Register a greedy constructor type
            container.Register<IFoo, Foo>(true);

            // With zero registered parameters, this should be the empty
            // constructor
            Foo result = (Foo) container.Resolve<IFoo>();

            Assert.NotEqual("My single string", result.MyString);
            Assert.NotEqual(42.0,               result.MyDouble);
            Assert.NotEqual(1234,               result.MyInt);

            // Register 2/3 of the things required for the big constructor
            container.Register<string>("My single string");
            container.Register<double>(42.0);

            result = (Foo) container.Resolve<IFoo>();

            // It should have picked just the string constructor because there
            // was a double, but no int, so the big constructor wasn't
            // satisfied
            Assert.Equal("My single string",    result.MyString);
            Assert.NotEqual(42.0,               result.MyDouble);
            Assert.NotEqual(1234,               result.MyInt);

            // Now register an int, this should have all the params necessary
            // for the big constructor
            container.Register<int>(1234);

            result = (Foo) container.Resolve<IFoo>();

            Assert.Equal("My single string",    result.MyString);
            Assert.Equal(42.0,                  result.MyDouble);
            Assert.Equal(1234,                  result.MyInt);
        }
    }
}
