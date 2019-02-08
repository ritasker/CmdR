using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CmdR
{
    public static class CmdRExtensions
    {
        public static void AddCmdR(this IServiceCollection services)
        {
            services.AddRouting();
            
            
            
        }
        
        public static void UseCmdR(this IApplicationBuilder app)
        {
            app.UseMiddleware<CmdRMiddleware>();
        }
    }
}