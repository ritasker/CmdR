using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public abstract class CommandHandler<TCommand> : ICommandHandler
    {
        protected CommandHandler(string path = "/")
        {
            Path = path;
        }

        public abstract Task Handle(TCommand command);

        protected HttpContext Context { get; set; }
        public string Path { get; }
    }

    public interface ICommandHandler
    {
        string Path { get; }
    }
}