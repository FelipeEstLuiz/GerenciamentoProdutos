using Application.Core.Command.Produto;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Produto;
public class UpdateProdutoCommandHandler(
    ILogger<UpdateProdutoCommandHandler> logger,
    ICategoriaCacheService categoriaCacheService,
    IProdutoRepository produtoRepository,
    IProdutoResponseService produtoResponseService
) : IRequestHandler<UpdateProdutoCommand, Result<ProdutoVo>>
{
    public async Task<Result<ProdutoVo>> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Entities.Produto? produto = await produtoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (produto is null)
                return Result<ProdutoVo>.Failure("Produto não encontrado.");

            IEnumerable<Domain.Entities.Produto> produtoPorNome = await produtoRepository.GetByNameAsync(request.Nome!, cancellationToken);

            if (produtoPorNome.Any(x => x.Nome == request.Nome! && x.Id != request.Id))
                return Result<ProdutoVo>.Failure($"Já existe um produto cadastrado com o nome {request.Nome}");

            IEnumerable<Domain.Entities.CategoriaProduto> categorias = await categoriaCacheService.GetAllAsync(cancellationToken);

            if (!categorias.Any(x => x.Id == request.Categoria!.Value))
                return Result<ProdutoVo>.Failure("Categoria inválida");

            produto.Update(
                request.Nome!,
                request.Descricao,
                request.Valor!.Value,
                request.QuantidadeEstoque!.Value,
                request.Categoria!.Value,
                request.Status!.Value,
                request.DataUltimaVenda
            );

            await produtoRepository.UpdateAsync(produto, cancellationToken);
            return await produtoResponseService.MontarProdutoVoAsync(produto, cancellationToken);
        }
        catch (ValidacaoException ex)
        {
            return Result<ProdutoVo>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar o produto {Id}", request.Id);
            return Result<ProdutoVo>.Failure("Erro ao atualizar o produto");
        }
    }
}
