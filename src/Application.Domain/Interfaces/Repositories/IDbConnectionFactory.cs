using System.Data;

namespace Application.Domain.Interfaces.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
