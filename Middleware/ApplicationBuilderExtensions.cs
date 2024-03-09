namespace LittleLemon_API.Middleware;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder SwaggerBasicAuthMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        return builder;
    }

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ErrorHandlingMiddleware>();
        return builder;
    }
}