using Application.Core.Dto;
using Application.Core.Queries.Produto;
using Application.Domain.Enums;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Produto;

public class GetProdutoByIdQueryHandler(
    ILogger<GetProdutoByIdQueryHandler> logger,
    IProdutoResponseService produtoResponseService,
    IProdutoRepository produtoRepository
) : IRequestHandler<GetProdutoByIdQuery, Result<ProdutoDto>>
{
    public async Task<Result<ProdutoDto>> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Entities.Produto? produto = await produtoRepository.GetByIdAsync(request.Id, cancellationToken);

            return produto is null
                ? Result<ProdutoDto>.Failure("Produto não encontrado.", ResponseCodes.NOT_FOUND)
                : new ProdutoDto(
                    produto.Id,
                    produto.Nome,
                    produto.Descricao,
                    produto.Valor,
                    produto.QuantidadeEstoque,
                    produto.IdCategoria,
                    produto.CodigoStatusProduto,
                    produto.DataCadastro,
                    produto.DataUltimaVenda
                );
        }
        catch (ValidacaoException ex)
        {
            return Result<ProdutoDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar o produto {Id}", request.Id);
            return Result<ProdutoDto>.Failure("Erro ao atualizar o produto");
        }
    }
}
