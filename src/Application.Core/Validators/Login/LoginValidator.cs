using Application.Core.Command.Login;
using FluentValidation;

namespace Application.Core.Validators.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Obrigatorio")
            .EmailAddress().WithMessage("Invalido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Obrigatorio");
    }
}
