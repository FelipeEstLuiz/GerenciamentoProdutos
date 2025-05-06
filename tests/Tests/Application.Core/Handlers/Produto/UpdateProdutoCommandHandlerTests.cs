using Application.Core.Command.Produto;
using Application.Core.Handlers.Produto;
using Application.Domain.Entities;
using Application.Domain.Enums;
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

public class UpdateProdutoCommandHandlerTests
{
    private readonly ICategoriaCacheService _categoriaCacheServiceMock;
    private readonly IProdutoRepository _produtoRepositoryMock;
    private readonly IProdutoResponseService _produtoResponseServiceMock;
    private readonly UpdateProdutoCommandHandler _handler;

    public UpdateProdutoCommandHandlerTests()
    {
        _categoriaCacheServiceMock = Substitute.For<ICategoriaCacheService>();
        _produtoRepositoryMock = Substitute.For<IProdutoRepository>();
        _produtoResponseServiceMock = Substitute.For<IProdutoResponseService>();

        _handler = new UpdateProdutoCommandHandler(
            Substitute.For<ILogger<UpdateProdutoCommandHandler>>(),
            _categoriaCacheServiceMock,
            _produtoRepositoryMock,
            _produtoResponseServiceMock
        );
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeProdutoNaoForEncontrado()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Atualizado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((ProdutoEntity?)null);

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Produto não encontrado.", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeProdutoComNomeDuplicadoExistir()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Duplicado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(new ProdutoEntity("Produto Original", "Descrição", 100.50m, 10, 1));

        _produtoRepositoryMock
            .GetByNameAsync(command.Nome!, Arg.Any<CancellationToken>())
            .Returns([new(2, "Produto Duplicado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)]);

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
        UpdateProdutoCommand command = new("Produto Atualizado", "Descrição", 100.50m, 10, 999, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(new ProdutoEntity("Produto Original", "Descrição", 100.50m, 10, 1));

        _produtoRepositoryMock
            .GetByNameAsync(command.Nome!, Arg.Any<CancellationToken>())
            .Returns([]);

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(1));

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Categoria inválida", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveAtualizarProdutoComSucesso()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Atualizado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        List<CategoriaProduto> categorias = CriarCategoria.CriarCategorias(1);

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(new ProdutoEntity("Produto Original", "Descrição", 100.50m, 10, 1));

        _produtoRepositoryMock
            .GetByNameAsync(command.Nome!, Arg.Any<CancellationToken>())
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
        await _produtoRepositoryMock.Received(1).UpdateAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeValidacaoExceptionForLancada()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Atualizado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(1));

        _produtoRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(new ProdutoEntity("Produto Original", "Descrição", 100.50m, 10, 1));

        _produtoRepositoryMock
            .When(x => x.UpdateAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>()))
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
        UpdateProdutoCommand command = new("Produto Atualizado", "Descrição", 100.50m, 10, 1, StatusProduto.Disponivel, null)
        {
            Id = 1
        };

        _categoriaCacheServiceMock
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(CriarCategoria.CriarCategorias(1));

        _produtoRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(new ProdutoEntity("Produto Original", "Descrição", 100.50m, 10, 1));

        _produtoRepositoryMock.When(x => x.UpdateAsync(Arg.Any<ProdutoEntity>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Erro inesperado"));

        // Act
        Result<ProdutoVo> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao atualizar o produto", result.Errors);
    }
}
