using Application.Core.Services;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Services;
using Application.Domain.VO;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tests.Util;

namespace Tests.Application.Core.Services;

public class ProdutoResponseServiceTests
{
    private readonly ICategoriaCacheService _categoriaCacheServiceMock;
    private readonly ProdutoResponseService _produtoResponseService;

    public ProdutoResponseServiceTests()
    {
        _categoriaCacheServiceMock = Substitute.For<ICategoriaCacheService>();
        _produtoResponseService = new ProdutoResponseService(_categoriaCacheServiceMock);
    }

    [Fact]
    public async Task MontarProdutoVoAsync_DeveRetornarProdutoVo_QuandoCategoriasExistem()
    {
        // Arrange
        Produto produto = new("Produto Teste", "Descrição Teste", 100.50m, 10, 1);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(2));

        // Act
        ProdutoVo result = await _produtoResponseService.MontarProdutoVoAsync(produto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produto.Nome, result.Nome);
        Assert.Equal(produto.Descricao, result.Descricao);
        Assert.Equal(produto.Valor, result.Valor);
        Assert.Equal(produto.QuantidadeEstoque, result.QuantidadeEstoque);
        Assert.Equal(produto.CodigoStatusProduto, result.Status);
    }

    [Fact]
    public async Task MontarProdutoVoAsync_DeveRetornarProdutoVo_QuandoNenhumaCategoriaExiste()
    {
        // Arrange
        Produto produto = new("Produto Teste", "Descrição Teste", 100.50m, 10, 1);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        ProdutoVo result = await _produtoResponseService.MontarProdutoVoAsync(produto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(produto.Nome, result.Nome);
        Assert.Equal(produto.Descricao, result.Descricao);
        Assert.Equal(produto.Valor, result.Valor);
        Assert.Equal(produto.QuantidadeEstoque, result.QuantidadeEstoque);
        Assert.Equal(produto.CodigoStatusProduto, result.Status);
    }

    [Fact]
    public async Task MontarProdutoVoAsync_DeveLancarExcecao_QuandoErroAoObterCategorias()
    {
        // Arrange
        Produto produto = new("Produto Teste", "Descrição Teste", 100.50m, 10, 1);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Throws(new Exception("Erro ao obter categorias"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _produtoResponseService.MontarProdutoVoAsync(produto, CancellationToken.None));
    }
}
