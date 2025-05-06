using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRepository();
        services.AddServices();
        return services;
    }
}
