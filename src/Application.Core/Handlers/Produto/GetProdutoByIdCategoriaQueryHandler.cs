using Application.Core.Dto;
using Application.Core.Queries.Produto;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Produto;

public class GetProdutoByIdCategoriaQueryHandler(
    ILogger<GetProdutoByIdCategoriaQueryHandler> logger,
    IProdutoRepository produtoRepository,
    IProdutoResponseService produtoResponseService
) : IRequestHandler<GetProdutoByIdCategoriaQuery, Result<IEnumerable<ProdutoVo>>>
{
    public async Task<Result<IEnumerable<ProdutoVo>>> Handle(GetProdutoByIdCategoriaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.Produto>? produtos = await produtoRepository.GetByCategoriaIdAsync(request.Id, cancellationToken);

            List<ProdutoVo> produtosVo = [];

            if (produtos.Any())
            {
                foreach (Domain.Entities.Produto produto in produtos)
                {
                    ProdutoVo produtoVo = await produtoResponseService.MontarProdutoVoAsync(produto, cancellationToken);
                    produtosVo.Add(produtoVo);
                }
            }

            return Result<IEnumerable<ProdutoVo>>.Success(produtosVo);
        }
        catch (ValidacaoException ex)
        {
            return Result<IEnumerable<ProdutoVo>>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter os produtos pela categoria {Id}", request.Id);
            return Result<IEnumerable<ProdutoVo>>.Failure("Erro ao obter os produtos pela categoria");
        }
    }
}
