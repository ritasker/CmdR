using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public class CmdRMiddleware
    {
        public Task InvokeAsync(HttpContext context)
        {
            return Task.CompletedTask;
        }
    }
}