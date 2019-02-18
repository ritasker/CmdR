using System.Threading.Tasks;
using CmdR;
using Microsoft.AspNetCore.Http;

namespace BasicSample.Features.Echo
{
    public class EchoHandler : PostCommandHandler<Echo>
    {
        protected override Task Handle(Echo command)
        {
            return Context.Response.WriteAsync($"echo: {command.Message}");
        }
    }
}