using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Dapper;
using System.Data;

namespace Application.Infraestructure.Data.Repository;

public class UsuarioRepository(IDbConnectionFactory dbConnectionFactory) : IUsuarioRepository
{
    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        string query = "SELECT * FROM Usuario WHERE Email = @Email";
        return await connection.QuerySingleOrDefaultAsync<Usuario>(new CommandDefinition(
            query, 
            new 
            { 
                Email = email 
            }, 
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        ));
    }
}