using Microsoft.Extensions.DependencyInjection;

namespace CmdR.Tests
{
    using FluentAssertions;
    using Validation;
    using Xunit;

    public class WhenAddingCmdR
    {
        private readonly ServiceCollection serviceCollection;

        public WhenAddingCmdR()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddCmdR();
        }

        [Fact]
        public void Should_Register_Validator_Locator()
        {
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IValidatorLocator));
        }
    }
}