using Application.Domain.Entities;
using Application.Domain.Enums;
using Application.Infraestructure.Data.Configuration;
using Application.Infraestructure.Data.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Application.Infraestructure.Data.Fixtures;
using Tests.Util;

namespace Tests.Application.Infraestructure.Data.Repository;

public class ProdutoRepositoryTests : IClassFixture<SqlServerTestFixture>, IAsyncLifetime
{
    private readonly SqlServerTestFixture _fixture;
    private readonly ILogger<ProdutoRepository> _loggerMock;
    private readonly ProdutoRepository _produtoRepository;

    public ProdutoRepositoryTests(SqlServerTestFixture fixture)
    {
        _fixture = fixture;

        DbConnectionFactory dbConnection = new(_fixture.Configuration);

        _loggerMock = Substitute.For<ILogger<ProdutoRepository>>();

        _produtoRepository = new ProdutoRepository(_loggerMock, dbConnection);
    }

    public async Task InitializeAsync() => await _fixture.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AddAsync_DeveAdicionarProduto()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();

        // Act
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        // Assert
        Assert.True(produto.Id > 0);
        Assert.Equal(StatusProduto.Disponivel, produto.CodigoStatusProduto);        
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProdutoPorId()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        // Act
        Produto? produtoObtido = await _produtoRepository.GetByIdAsync(produto.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(produtoObtido);
        Assert.Equal(produto.Id, produtoObtido!.Id);
        Assert.Equal(produto.Nome, produtoObtido.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNullProdutoPorId()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        // Act
        Produto? produtoObtido = await _produtoRepository.GetByIdAsync(200, CancellationToken.None);

        // Assert
        Assert.Null(produtoObtido);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProduto()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        produto.Update("Produto Atualizado", "Descrição Atualizada", 200.00m, 20, 2, StatusProduto.ForaEstoque, DateTime.UtcNow.AddDays(-61));

        // Act
        await _produtoRepository.UpdateAsync(produto, CancellationToken.None);

        // Assert
        Produto? produtoAtualizado = await _produtoRepository.GetByIdAsync(produto.Id, CancellationToken.None);

        Assert.NotNull(produtoAtualizado);
        Assert.Equal("Produto Atualizado", produtoAtualizado!.Nome);
        Assert.Equal(200.00m, produtoAtualizado.Valor);
        Assert.Equal(StatusProduto.ForaEstoque, produtoAtualizado.CodigoStatusProduto);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverProduto()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        // Act
        await _produtoRepository.DeleteAsync(produto.Id, CancellationToken.None);

        // Assert
        Produto? produtoRemovido = await _produtoRepository.GetByIdAsync(produto.Id, CancellationToken.None);
        Assert.Null(produtoRemovido);
    }

    [Fact]
    public async Task UpdateStatusForOldProductsAsync_DeveAtualizarStatusDeProdutosAntigos()
    {
        // Arrange
        Produto produto = CriarProduto.CriarProdutoTeste();
        produto.SetDataUltimaVenda(DateTime.UtcNow.AddDays(-61));
        await _produtoRepository.AddAsync(produto, CancellationToken.None);

        produto.Update("Produto Atualizado", "Descrição Atualizada", 200.00m, 20, 2, StatusProduto.Disponivel, DateTime.UtcNow.AddDays(-61));
        await _produtoRepository.UpdateAsync(produto, CancellationToken.None);

        // Act
        await _produtoRepository.UpdateStatusForOldProductsAsync(CancellationToken.None);

        // Assert
        Produto? produtoAtualizado = await _produtoRepository.GetByIdAsync(produto.Id, CancellationToken.None);
        Assert.NotNull(produtoAtualizado);
        Assert.Equal(StatusProduto.ForaEstoque, produtoAtualizado!.CodigoStatusProduto);
    }
}
