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
            var routeBuilder = new RouteBuilder(builder);
            
            foreach (var handler in builder.ApplicationServices.GetServices<ICommandHandler>())
            {
                var handlerType = handler.GetType().BaseType?.GetGenericTypeDefinition();
                
                if (handlerType == typeof(PostCommandHandler<>))
                {
                    routeBuilder.MapPost(handler.Path, ctx => handler.HandleRequest(ctx));
                }
                if (handlerType == typeof(PutCommandHandler<>))
                {
                    routeBuilder.MapPut(handler.Path, ctx => handler.HandleRequest(ctx));
                }
            }

            builder.UseRouter(routeBuilder.Build());
        }
    }
}