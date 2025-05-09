using Application.Core.Services;
using Application.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;

public static class DependencyInjectionServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioCacheService, UsuarioCacheService>();
        services.AddScoped<ICategoriaCacheService, CategoriaCacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProdutoResponseService, ProdutoResponseService>();
        return services;
    }
}
