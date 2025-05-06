using Application.Core.Command.Produto;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Produto;

public class CreateProdutoCommandHandler(
    ILogger<CreateProdutoCommandHandler> logger,
    ICategoriaCacheService categoriaCacheService,
    IProdutoRepository produtoRepository,
    IProdutoResponseService produtoResponseService
) : IRequestHandler<CreateProdutoCommand, Result<ProdutoVo>>
{
    public async Task<Result<ProdutoVo>> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.Produto> produtoPorNome = await produtoRepository.GetByNameAsync(request.Nome!, cancellationToken);

            if (produtoPorNome.Any(x => x.Nome == request.Nome!))
                return Result<ProdutoVo>.Failure($"Já existe um produto cadastrado com o nome {request.Nome}");

            IEnumerable<Domain.Entities.CategoriaProduto> categorias = await categoriaCacheService.GetAllAsync(cancellationToken);

            if (!categorias.Any(x => x.Id == request.Categoria!.Value))
                return Result<ProdutoVo>.Failure("Categoria inválida");

            Domain.Entities.Produto produto = new(
                request.Nome!,
                request.Descricao,
                request.Valor!.Value,
                request.QuantidadeEstoque!.Value,
                request.Categoria!.Value
            );

            await produtoRepository.AddAsync(produto, cancellationToken);
            return await produtoResponseService.MontarProdutoVoAsync(produto, cancellationToken);
        }
        catch (ValidacaoException ex)
        {
            return Result<ProdutoVo>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao cadastrar o produto");
            return Result<ProdutoVo>.Failure("Erro ao cadastrar o produto");
        }
    }
}
