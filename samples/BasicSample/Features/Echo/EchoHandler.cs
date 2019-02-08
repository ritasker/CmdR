using System.Threading.Tasks;
using CmdR;
using Microsoft.AspNetCore.Http;

namespace BasicSample.Features.Echo
{
    public class EchoHandler : CommandHandler<Echo>
    {
        public override async Task Handle(Echo command)
        {
            await Context.Response.WriteAsync($"echo: {command.Message}");
        }
    }
}