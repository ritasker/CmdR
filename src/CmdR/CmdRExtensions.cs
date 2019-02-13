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
                services.AddScoped(typeof(ICommandHandler),handler);
            }
        }

        public static void UseCmdR(this IApplicationBuilder app)
        {
             app.UseMiddleware<CmdRMiddleware>();
        }
    }
}