using Application.Domain.Model;
using MediatR;

namespace Application.Core.Command.Produto;
public record DeleteProdutoCommand(int Id) : IRequest<Result<string>>;