using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Dapper;
using System.Data;

namespace Application.Infraestructure.Data.Repository;

public class CategoriaRepository(IDbConnectionFactory dbConnectionFactory) : ICategoriaRepository
{
    public async Task<IEnumerable<CategoriaProduto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        string query = "SELECT Id, Descricao FROM CategoriaProduto";
        return await connection.QueryAsync<CategoriaProduto>(new CommandDefinition(
            query,
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        ));
    }
}
