using Application.Core.Command.Login;
using Application.Domain.Exceptions;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Domain.Util;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.Handlers.Login;

public class LoginCommandHanlder(
    ILogger<LoginCommandHanlder> logger,
    IUsuarioRepository usuarioRepository,
    ITokenService tokenService
) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.Entities.Usuario? usuario = await usuarioRepository.GetByEmailAsync(request.Email!, cancellationToken);

            if (usuario is not null)
            {
                if (!ClsGlobal.ValidarSenha(request.Senha!, usuario.Senha))
                    return Result<string>.Failure("Usuário/Senha inválida");

               return await tokenService.GerarToken(usuario);
            }

            return Result<string>.Failure("Usuário/Senha inválida");
        }
        catch (ValidacaoException ex)
        {
            return Result<string>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao realizar login {Email}", request.Email);
            return Result<string>.Failure("Erro ao realizar login");
        }
    }
}
