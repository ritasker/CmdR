using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public abstract class CommandHandler<TIn>
    {
        protected CommandHandler(string path = "/")
        {
            Path = path;
        }

        public abstract Task Handle(TIn command);

        protected HttpContext Context { get; set; }
        protected string Path { get; }
    }
}