using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR.Handler;

public abstract class BaseHandler : IHandler
{
    internal abstract Task ExecuteAsync(HttpContext ctx, CancellationToken ct);

    public abstract void Configure();

    internal string Route { get; private set; }

    internal string[] Verbs { get; private set; }

    protected void Get(string route)
    {
        Route = route;
        Verbs = new[] { HttpMethod.Get.Method };
    }

    protected void Post(string route)
    {
        Route = route;
        Verbs = new[] { HttpMethod.Post.Method };
    }

    protected void Put(string route)
    {
        Route = route;
        Verbs = new[] { HttpMethod.Put.Method };
    }

    protected void Delete(string route)
    {
        Route = route;
        Verbs = new[] { HttpMethod.Delete.Method };
    }
}