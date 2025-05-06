using Application.Domain.Model;
using MediatR;

namespace Application.Core.Command.Login;

public record LoginCommand(string? Email, string? Senha) : IRequest<Result<string>>;
