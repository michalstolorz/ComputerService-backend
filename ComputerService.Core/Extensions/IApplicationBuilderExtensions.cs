using ComputerService.Core.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace ComputerService.Core.Extensions
{
    public static class Extensions
    {
        public static IApplicationBuilder UseDevelopmentExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware(typeof(DevelopmentExceptionHandlerMiddleware));
        public static IApplicationBuilder UseProductionExceptionHandler(this IApplicationBuilder builder)
           => builder.UseMiddleware(typeof(ProductionExceptionHandlerMiddleware));
    }
}
