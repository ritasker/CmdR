using CmdR.Tests.TestClasses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CmdR.Tests
{
    public class WhenAddingCmdRDependencies
    {
        private readonly IServiceCollection services;
        
        public WhenAddingCmdRDependencies()
        {
            services = new ServiceCollection();
            services.AddCmdR();
        }
        
        [Fact]
        public void ShouldRegisterHandlers()
        {
            services.Should().Contain(x => x.ImplementationType == typeof(TestHandler));
        }
    }
}
