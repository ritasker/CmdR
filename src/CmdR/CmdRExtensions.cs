using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace CmdR
{
    using Microsoft.AspNetCore.Routing;
    using Validation;


    public static class CmdRExtensions
    {
        public static void AddCmdR(this IServiceCollection services)
        {
            services.AddRouting();
            services.AddSingleton<IValidatorLocator, ValidatorLocator>();
            
            RegisterCommandValidators(services);
            RegisterCommandHandlers(typeof(PostCommandHandler<>), services);
            RegisterCommandHandlers(typeof(PutCommandHandler<>), services);
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

        private static void RegisterCommandHandlers(Type commandHandlerType, IServiceCollection services)
        {
            var handlers = Assembly.GetEntryAssembly().GetTypes()
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == commandHandlerType);

            foreach (var handler in handlers)
            {
                services.AddSingleton(typeof(ICommandHandler), handler);
            }
        }

        public static void UseCmdR(this IApplicationBuilder builder)
        {
            builder.UseRouting();
            builder.UseEndpoints(cfg =>
            {
                foreach (var handler in cfg.ServiceProvider.GetServices<ICommandHandler>())
                {
                    if (handler.GetType().BaseType.GetGenericTypeDefinition() == typeof(PostCommandHandler<>))
                    {
                        cfg.MapPost(handler.Path, ctx => handler.HandleRequest(ctx));
                    }
                    
                    if (handler.GetType().BaseType.GetGenericTypeDefinition() == typeof(PutCommandHandler<>))
                    {
                        cfg.MapPut(handler.Path, ctx => handler.HandleRequest(ctx));
                    }
                }
            });
        }
    }
}