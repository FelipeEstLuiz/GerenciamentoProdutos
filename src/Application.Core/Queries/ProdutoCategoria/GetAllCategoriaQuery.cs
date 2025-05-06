using Application.Core.Dto;
using Application.Domain.Model;
using MediatR;

namespace Application.Core.Queries.ProdutoCategoria;
public record GetAllCategoriaQuery() : IRequest<Result<IEnumerable<CategoriaProdutoDto>>>;
