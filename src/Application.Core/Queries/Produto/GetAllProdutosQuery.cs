using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Queries.Produto;

public record GetAllProdutosQuery(int? IdCategoria, string? Nome) : IRequest<Result<IEnumerable<ProdutoVo>>>;