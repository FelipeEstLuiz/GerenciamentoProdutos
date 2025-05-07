using Application.Core.Command.Produto;
using Application.Core.Handlers.Produto;
using Application.Domain.Entities;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Util;
using ProdutoEntity = Application.Domain.Entities.Produto;

namespace Tests.Application.Core.Handlers.Produto;

public class CreateProdutoCommandHandlerTests
{
    private readonly ILogger<CreateProdutoCommandHandler> _loggerMock;
    private readonly ICategoriaCacheService _categoriaCacheServiceMock;
    private readonly IProdutoRepository _produtoRepositoryMock;
    private readonly IProdutoResponseService _produtoResponseServiceMock;
    private readonly CreateProdutoCommandHandler _handler;

    public CreateProdutoCommandHandlerTests()
    {
        _loggerMock = Substitute.For<ILogger<CreateProdutoCommandHandler>>();
        _categoriaCacheServiceMock = Substitute.For<ICategoriaCacheService>();
        _produtoRepositoryMock = Substitute.For<IProdutoRepository>();
        _produtoResponseServiceMock = Substitute.For<IProdutoResponseService>();

        _handler = new CreateProdutoCommandHandler(
            _loggerMock,
            _categoriaCacheServiceMock,
            _produtoRepositoryMock,
            _produtoResponseServiceMock
        );
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeProdutoJaExistir()
    {
        // Arrange

        ProdutoEntity produto = CriarProduto.CriarProdutoTeste();

        CreateProdutoCommand command = new(produto.Nome, produto.Descricao, produto.Valor, produto.QuantidadeEstoque, produto.IdCategoria);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(1));

        _produtoRepositoryMock
            .GetAllAsync(Arg.Any<int?>(), command.Nome!, Arg.Any<CancellationToken>())
            .Returns([produto]);

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains($"Já existe um produto cadastrado com o nome {command.Nome}", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeCategoriaForInvalida()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Novo", "Descrição", 100.50m, 10, 999); // Categoria inválida

        _produtoRepositoryMock
            .GetAllAsync(Arg.Any<int?>(), command.Nome!, Arg.Any<CancellationToken>())
            .Returns([]);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(1));

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);

    }

    [Fact]
    public async Task Handle_DeveCriarProdutoComSucesso()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Novo", "Descrição", 100.50m, 10, 1);

        List<CategoriaProduto> categorias = CriarCategoria.CriarCategorias(1);

        _produtoRepositoryMock
            .GetAllAsync(Arg.Any<int?>(), command.Nome!, Arg.Any<CancellationToken>())
            .Returns([]);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(categorias);

        ProdutoVo produtoVo = ProdutoVo.FromEntity(new(
            command.Nome!,
            command.Descricao,
            command.Valor!.Value,
            command.QuantidadeEstoque!.Value,
            1
        ), categorias);

        _produtoResponseServiceMock
            .MontarProdutoVoAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>())
            .Returns(produtoVo);

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(produtoVo, result.Data);
        await _produtoRepositoryMock.Received(1).AddAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeValidacaoExceptionForLancada()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Novo", "Descrição", 100.50m, 10, 1);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
           .Returns(CriarCategoria.CriarCategorias(1));

        _produtoRepositoryMock
            .When(x => x.AddAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new ValidacaoException("Erro de validação"));

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Erro de validação", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeErroInesperadoForLancado()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Novo", "Descrição", 100.50m, 10, 1);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
           .Returns(CriarCategoria.CriarCategorias(1));

        _produtoRepositoryMock
            .When(x => x.AddAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Erro inesperado"));

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao cadastrar o produto", result.Errors);
        _loggerMock.Received(1).LogError(Arg.Any<Exception>(), "Erro ao cadastrar o produto");
    }
}
