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
    internal override async Task ExecuteAsync(HttpContext ctx, CancellationToken ct)
    {
        var request = await BindAsync(ctx, ct);
        await ctx.Response.WriteAsJsonAsync(await HandleAsync(request, ct), ct);
    }

    private async Task<TRequest> BindAsync(HttpContext ctx, CancellationToken ct)
    {
        TRequest? request = default;
        if (ctx.Request.HasJsonContentType())
        {
            request = await ctx.Request.ReadFromJsonAsync<TRequest>(ct);
        }

        return request;
    }

    public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
}