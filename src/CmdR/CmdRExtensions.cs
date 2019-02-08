using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CmdR
{
    public static class CmdRExtensions
    {
        public static void AddCmdR(this IServiceCollection services)
        {
            RegisterCommandValidators(services);
            RegisterCommandHandlers(services);
        }

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            var handlers = Assembly.GetEntryAssembly().GetTypes().ToList()
                .Where(typeof(CommandHandler<>).IsAssignableFrom)
                .Where(t => !t.GetTypeInfo().IsAbstract);

            foreach (var handler in handlers)
            {
                services.AddSingleton(typeof(CommandHandler<>), handler);
            }
        }

        private static void RegisterCommandValidators(IServiceCollection services)
        {
            var validators = Assembly.GetEntryAssembly().GetTypes().ToList()
                .Where(typeof(IValidator).IsAssignableFrom)
                .Where(t => !t.GetTypeInfo().IsAbstract);

            foreach (var validator in validators)
            {
                services.AddSingleton(typeof(IValidator), validator);
            }
        }

        public static void UseCmdR(this IApplicationBuilder app)
        {
            app.UseMiddleware<CmdRMiddleware>();
        }
    }
}