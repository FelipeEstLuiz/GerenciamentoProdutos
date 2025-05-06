using Application.Core.Dto;
using Application.Core.Queries.ProdutoCategoria;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Categoria;

public class GetAllCategoriaQueryHandler(
    ILogger<GetAllCategoriaQueryHandler> logger,
    ICategoriaCacheService categoriaCacheService
) : IRequestHandler<GetAllCategoriaQuery, Result<IEnumerable<CategoriaProdutoDto>>>
{
    public async Task<Result<IEnumerable<CategoriaProdutoDto>>> Handle(GetAllCategoriaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.CategoriaProduto> categorias = await categoriaCacheService.GetAllAsync(cancellationToken);

            return Result<IEnumerable<CategoriaProdutoDto>>.Success(categorias.Select(cat => new CategoriaProdutoDto(cat.Id, cat.Descricao)));
        }
        catch (ValidacaoException ex)
        {
            return Result<IEnumerable<CategoriaProdutoDto>>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter as categorias");
            return Result<IEnumerable<CategoriaProdutoDto>>.Failure("Erro ao obter as categorias");
        }
    }
}
