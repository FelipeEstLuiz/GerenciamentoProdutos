using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Queries.Produto;

public record GetProdutoByNomeQuery(string Nome) : IRequest<Result<IEnumerable<ProdutoVo>>>;
