using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Core.Services;

public class CategoriaCacheService(ICategoriaRepository categoriaRepository, IMemoryCache memoryCache) : ICategoriaCacheService
{
    public async Task<IEnumerable<CategoriaProduto>> GetAllAsync(CancellationToken cancellationToken)
    {
        string key = "categoria_cache";

        if (memoryCache.TryGetValue(key, out IEnumerable<CategoriaProduto>? categoria))
            return categoria!;

        categoria = await categoriaRepository.GetAllAsync(cancellationToken);

        memoryCache.Set(key, categoria, TimeSpan.FromMinutes(30));

        return categoria;
    }
}
