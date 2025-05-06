using Application.Core.Command.Produto;
using Application.Core.Handlers.Produto;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Util;
using ProdutoEntity = Application.Domain.Entities.Produto;

namespace Tests.Application.Core.Handlers.Produto;

public class DeleteProdutoCommandHandlerTests
{
    private readonly IProdutoRepository _produtoRepositoryMock;
    private readonly DeleteProdutoCommandHandler _handler;

    public DeleteProdutoCommandHandlerTests()
    {
        _produtoRepositoryMock = Substitute.For<IProdutoRepository>();
        _handler = new DeleteProdutoCommandHandler(Substitute.For<ILogger<DeleteProdutoCommandHandler>>(), _produtoRepositoryMock);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeProdutoNaoForEncontrado()
    {
        // Arrange
        DeleteProdutoCommand command = new(1);

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((ProdutoEntity?)null);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Produto não encontrado.", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveRemoverProdutoComSucesso()
    {
        // Arrange
        DeleteProdutoCommand command = new(1);
        ProdutoEntity produto = CriarProduto.CriarProdutoTeste();

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(produto);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Produto removido com sucesso", result.Data);
        await _produtoRepositoryMock.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeValidacaoExceptionForLancada()
    {
        // Arrange
        DeleteProdutoCommand command = new(1);

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(CriarProduto.CriarProdutoTeste());

        _produtoRepositoryMock
            .When(x => x.DeleteAsync(command.Id, Arg.Any<CancellationToken>()))
            .Do(x => throw new ValidacaoException("Erro de validação"));

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Erro de validação", result.Errors);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_SeErroInesperadoForLancado()
    {
        // Arrange
        DeleteProdutoCommand command = new(1);

        _produtoRepositoryMock
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(CriarProduto.CriarProdutoTeste());

        _produtoRepositoryMock
            .When(x => x.DeleteAsync(command.Id, Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Erro inesperado"));

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao remover o produto", result.Errors);
    }
}
