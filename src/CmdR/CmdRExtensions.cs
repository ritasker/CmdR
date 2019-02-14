using System.Linq;
using System.Reflection;
using CmdR.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CmdR
{
    public static class CmdRExtensions
    {
        public static void AddCmdR(this IServiceCollection services)
        {
            services.AddSingleton<IValidatorLocator, ValidatorLocator>();
            
            RegisterCommandValidators(services);
            RegisterCommandHandlers(services);
        }

        private static void RegisterCommandValidators(IServiceCollection services)
        {
            var validators = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => typeof(IValidator).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);

            foreach (var validator in validators)
            {
                services.AddSingleton(typeof(IValidator), validator);
            }
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

        public static void UseCmdR(this IApplicationBuilder builder)
        {
            builder.UseRouting(cfg =>
            {
                foreach (var handler in cfg.ServiceProvider.GetServices<ICommandHandler>())
                {
                    cfg.MapPost(handler.Path, ctx => handler.HandleRequest(ctx));
                }
            });
        }
    }
}