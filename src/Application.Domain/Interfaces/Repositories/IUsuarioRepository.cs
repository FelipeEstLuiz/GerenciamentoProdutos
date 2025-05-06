using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
