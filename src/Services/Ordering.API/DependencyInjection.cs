namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // TODO: Add Carter

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // TODO: mapCarter

        return app;
    }
}