using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<CategoriaProduto>> GetAllAsync(CancellationToken cancellationToken);
}
