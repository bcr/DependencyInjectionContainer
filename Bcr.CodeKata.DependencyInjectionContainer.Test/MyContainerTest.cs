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

        public string MyString { get; private set; }
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
    }
}
