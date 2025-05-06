using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Queries.Produto;
public record GetProdutoByIdCategoriaQuery(int Id) : IRequest<Result<IEnumerable<ProdutoVo>>>;
