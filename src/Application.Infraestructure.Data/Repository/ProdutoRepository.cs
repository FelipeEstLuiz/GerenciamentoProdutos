using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Infraestructure.Data.Repository;

public class ProdutoRepository(
    ILogger<ProdutoRepository> logger,
    IDbConnectionFactory dbConnectionFactory
) : IProdutoRepository
{
    private const string QueryBase = @"
        SELECT 
            Id,
            Nome,
            CodigoStatusProduto,
            IdCategoria,
            Valor,
            QuantidadeEstoque,
            DataCadastro,
            Descricao,
            DataUltimaVenda 
        FROM Produto
    ";

    public async Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string query = QueryBase + " WHERE Id = @Id";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Produto>(new CommandDefinition(
                query,
                new
                {
                    Id = id
                }, cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter o produto");
            throw new ValidacaoException("Erro ao obter o produto");
        }
    }

    public async Task<IEnumerable<Produto>> GetByNameAsync(string nome, CancellationToken cancellationToken)
    {
        const string query = QueryBase + " WHERE Nome LIKE @Nome ";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Produto>(new CommandDefinition(
                query,
                new
                {
                    Nome = $"%{nome}%"
                }, cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter o produto");
            throw new ValidacaoException("Erro ao obter o produto");
        }
    }

    public async Task<IEnumerable<Produto>> GetByCategoriaIdAsync(int id, CancellationToken cancellationToken)
    {
        const string query = QueryBase + " WHERE IdCategoria = @Id ";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Produto>(new CommandDefinition(
                query,
                new
                {
                    Id = id
                }, cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter o produto");
            throw new ValidacaoException("Erro ao obter o produto");
        }
    }

    public async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<Produto>(new CommandDefinition(QueryBase, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter os produto");
            throw new ValidacaoException("Erro ao obter os produto");
        }
    }

    public async Task AddAsync(Produto produto, CancellationToken cancellationToken)
    {
        const string query = @"
            INSERT INTO Produto (Nome, Descricao, Valor, QuantidadeEstoque, IdCategoria)
            OUTPUT INSERTED.Id, INSERTED.CodigoStatusProduto, INSERTED.DataCadastro
            VALUES (@Nome, @Descricao, @Valor, @QuantidadeEstoque, @IdCategoria)
        ";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();

            dynamic response = await connection.QuerySingleAsync<dynamic>(new CommandDefinition(query, new
            {
                produto.Nome,
                produto.Descricao,
                produto.Valor,
                produto.QuantidadeEstoque,
                produto.IdCategoria
            }, cancellationToken: cancellationToken));

            produto.SetId(response.Id);
            produto.SetStatus((StatusProduto)response.CodigoStatusProduto);
            produto.SetDataCadastro((DateTime)response.DataCadastro);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inserir produto");
            throw new ValidacaoException("Erro ao inserir produto");
        }
    }

    public async Task UpdateAsync(Produto produto, CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE Produto
            SET Nome = @Nome,
                Descricao = @Descricao,
                Valor = @Valor,
                QuantidadeEstoque = @QuantidadeEstoque,
                IdCategoria = @IdCategoria,
                CodigoStatusProduto = @CodigoStatusProduto,
                DataUltimaVenda = @DataUltimaVenda
            WHERE Id = @Id
        ";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(new CommandDefinition(query, new
            {
                produto.Id,
                produto.Nome,
                produto.Descricao,
                produto.Valor,
                produto.QuantidadeEstoque,
                produto.IdCategoria,
                CodigoStatusProduto = (int)produto.CodigoStatusProduto,
                produto.DataUltimaVenda
            }, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar o produto");
            throw new ValidacaoException("Erro ao atualizar o produto");
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        const string query = "DELETE FROM Produto WHERE Id = @Id";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(new CommandDefinition(
                query,
                new
                {
                    Id = id
                },
                cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover o  produto");
            throw new ValidacaoException("Erro ao remover o produto");
        }
    }

    public async Task UpdateStatusForOldProductsAsync(CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE Produto
            SET CodigoStatusProduto = @NovoStatus
            WHERE CodigoStatusProduto = @StatusAtual
            AND CAST(DataUltimaVenda AS DATE) <= CAST(@DataConsulta AS DATE)
        ";

        try
        {
            using IDbConnection connection = dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(new CommandDefinition(
                query,
                new
                {
                    NovoStatus = (int)StatusProduto.ForaEstoque,
                    StatusAtual = (int)StatusProduto.Disponivel,
                    DataConsulta = DateTime.UtcNow.AddDays(-60)
                },
                cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar o status dos produtos.");
            throw new ValidacaoException("Erro ao atualizar o status dos produtos.");
        }
    }
}
