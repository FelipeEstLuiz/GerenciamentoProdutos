using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Core.Services;

public class UsuarioCacheService(IUsuarioRepository usuarioRepository, IMemoryCache memoryCache) : IUsuarioCacheService
{
    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        if (memoryCache.TryGetValue(email, out Usuario? usuario))
            return usuario;

        usuario = await usuarioRepository.GetByEmailAsync(email, cancellationToken);

        if (usuario != null)
            memoryCache.Set(email, usuario, TimeSpan.FromMinutes(30));

        return usuario;
    }
}
