using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR
{
    public class CmdRMiddleware
    {
        private readonly RequestDelegate _next;

        public CmdRMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public Task InvokeAsync(HttpContext context)
        {
            return _next(context);
        }
    }
}