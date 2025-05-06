using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Services;

public  interface IUsuarioCacheService
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
