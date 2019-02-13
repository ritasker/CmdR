using System.IO;
using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public abstract class CommandHandler<TCommand> : ICommandHandler
    {
        protected CommandHandler(string path = "/")
        {
            Path = path;
        }

        public Task HandleRequest(HttpContext context)
        {
            Context = context;
            var command = GetCommand(context);

            // Validate

            return Handle(command);
        }

        private static TCommand GetCommand(HttpContext context)
        {
            using (var input = new StringReader(AsString(context.Request.Body)))
            {
                return JSON.Deserialize<TCommand>(input);
            }
        }

        protected abstract Task Handle(TCommand command);

        public string Path { get; }
        protected HttpContext Context { get; private set; }


        private static string AsString(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}