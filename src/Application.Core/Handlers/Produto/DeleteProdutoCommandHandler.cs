using Application.Core.Command.Produto;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Produto;
public class DeleteProdutoCommandHandler(
    ILogger<DeleteProdutoCommandHandler> logger,
    IProdutoRepository produtoRepository
) : IRequestHandler<DeleteProdutoCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Entities.Produto? produto = await produtoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (produto is null)
                return Result<string>.Failure("Produto não encontrado.");

            await produtoRepository.DeleteAsync(request.Id, cancellationToken);
            return Result<string>.Success("Produto removido com sucesso");
        }
        catch (ValidacaoException ex)
        {
            return Result<string>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover o produto {Id}", request.Id);
            return Result<string>.Failure("Erro ao remover o produto");
        }
    }
}
