using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Services;

public interface ICategoriaCacheService
{
    Task<IEnumerable<CategoriaProduto>> GetAllAsync(CancellationToken cancellationToken);
}
