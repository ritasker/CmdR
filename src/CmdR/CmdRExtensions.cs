using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CmdR
{
    public static class CmdRExtensions
    {
        public static void AddCmdR(this IServiceCollection services)
        {
            RegisterCommandHandlers(services);
        }

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            var handlers = Assembly.GetEntryAssembly().GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(CommandHandler<>));

            foreach (var handler in handlers)
            {
                services.AddSingleton(typeof(ICommandHandler),handler);
            }
        }

        public static IApplicationBuilder UseCmdR(this IApplicationBuilder builder)
        {
            return builder.UseRouting(cfg =>
            {
                foreach (var handler in cfg.ServiceProvider.GetServices<ICommandHandler>())
                {
                    cfg.MapPost(handler.Path, ctx => handler.HandleRequest(ctx));
                }
            });
        }
    }
}