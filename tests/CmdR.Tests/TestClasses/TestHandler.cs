using System.Threading.Tasks;

namespace CmdR.Tests.TestClasses
{
    public class TestHandler : CommandHandler<string>
    {
        protected override Task Handle(string command)
        {
            return Task.CompletedTask;
        }
    }
}