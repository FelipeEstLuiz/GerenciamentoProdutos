using Application.Core.Dto;
using Application.Domain.Model;
using Application.Domain.VO;
using MediatR;

namespace Application.Core.Queries.Produto;
public record GetProdutoByIdQuery(int Id) : IRequest<Result<ProdutoDto>>;
