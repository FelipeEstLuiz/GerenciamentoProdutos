using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Configuration;
using Application.Infraestructure.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infraestructure.IOC;
internal static class DependencyInjectionRepository
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        return services;
    }
}
