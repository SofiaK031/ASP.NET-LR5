namespace LR5;
using LR5.Models;

public static class AppExtensionBuilder
{
    public static void UseMiddlewareExceptionLogger(this IApplicationBuilder app)
    {
        app.UseMiddleware<MiddlewareErrorLogger>();
    }
}
