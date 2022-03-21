using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CmdR.Handler;

public abstract class RequestHandler<TResponse> : BaseHandler
{
    internal override async Task ExecuteAsync(HttpContext ctx, CancellationToken ct)
    {
        await ctx.Response.WriteAsJsonAsync(await HandleAsync(ct), ct);
    }

    public abstract Task<TResponse> HandleAsync(CancellationToken ct);
}

public abstract class RequestHandler<TRequest, TResponse> : BaseHandler
{
    internal override Task ExecuteAsync(HttpContext ctx, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
}