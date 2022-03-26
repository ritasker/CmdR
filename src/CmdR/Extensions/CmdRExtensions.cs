using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CmdR.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CmdR.Extensions;

public static class CmdRExtensions
{
    public static IServiceCollection AddCmdR(this IServiceCollection services, Type? type = null)
    {
        var assembly = type?.Assembly ?? Assembly.GetExecutingAssembly();
        foreach (var handler in GetHandlers(assembly))
        {
            services.AddTransient(typeof(IHandler), handler);
        }
        
        return services;
    }

    public static IEndpointRouteBuilder UseCmdR(this IEndpointRouteBuilder builder)
    {
        foreach (BaseHandler handler in builder.ServiceProvider.GetServices<IHandler>())
        {
            handler.Configure();
            var handlerBuilder = builder.MapMethods(handler.Route, handler.Verbs, handler.ExecuteAsync);
        }
        
        return builder;
    }
        
    private static IEnumerable<Type> GetHandlers(Assembly assembly)
    {
        var handlers = assembly.GetTypes().Where(t =>
            !t.IsAbstract &&
            typeof(IHandler).IsAssignableFrom(t) &&
            t != typeof(IHandler) &&
            t.IsPublic
        );

        return handlers;
    }
}