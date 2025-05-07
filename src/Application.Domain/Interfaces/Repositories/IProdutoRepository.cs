using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Repositories;

public interface IProdutoRepository
{
    Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Produto>> GetAllAsync(int? idCategoria, string? nome, CancellationToken cancellationToken);
    Task AddAsync(Produto produto, CancellationToken cancellationToken);
    Task UpdateAsync(Produto produto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task UpdateStatusForOldProductsAsync(CancellationToken cancellationToken);
}
