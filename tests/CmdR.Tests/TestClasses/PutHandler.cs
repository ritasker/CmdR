using System.IO;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Http;

namespace CmdR.Tests.TestClasses
{
    public class PutHandler : PutCommandHandler<BasicCommand>
    {
        protected override Task Handle(BasicCommand command)
        {
            using(var output = new StringWriter())
            {
                JSON.Serialize(command,output);
                return Context.Response.WriteAsync($"Echo: {output}");
            }
        }
    }
}