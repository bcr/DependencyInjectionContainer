using System;
using Xunit;

namespace Bcr.CodeKata.DependencyInjectionContainer.Test
{
    public class MyContainerTest
    {
        [Fact]
        public void container_should_be_creatable()
        {
            var container = new MyContainer();
            Assert.NotNull(container);
        }
    }
}
