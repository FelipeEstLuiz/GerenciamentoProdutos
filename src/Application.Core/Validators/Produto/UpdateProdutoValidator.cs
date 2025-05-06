using Application.Core.Command.Produto;
using FluentValidation;

namespace Application.Core.Validators.Produto;

public class UpdateProdutoValidator : AbstractValidator<UpdateProdutoCommand>
{
    public UpdateProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Obrigatorio")
            .Length(5, 250).WithMessage("Deve ter entre 5 e 250 caracter");

        RuleFor(x => x.Descricao)
            .Length(5, 800).WithMessage("Deve ter entre 5 e 800 caracter")
            .When(x => !string.IsNullOrEmpty(x.Descricao));

        RuleFor(x => x.Valor)
            .NotEmpty().WithMessage("Obrigatorio")
            .GreaterThan(0).WithMessage("Deve ser maior que 0");

        RuleFor(x => x.QuantidadeEstoque)
            .NotEmpty().WithMessage("Obrigatorio")
            .GreaterThan(0).WithMessage("Deve ser maior que 0");

        RuleFor(x => x.Categoria)
            .NotEmpty().WithMessage("Obrigatorio");

        RuleFor(x => x.Id)
           .NotEmpty().WithMessage("Obrigatorio")
           .GreaterThan(0).WithMessage("Invalido");

        RuleFor(x => x.Status)
           .NotEmpty().WithMessage("Obrigatorio")
           .IsInEnum().WithMessage("Invalido");

        RuleFor(x => x.DataUltimaVenda)
           .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Nao pode ser posterior a data atual")
           .When(x => x.DataUltimaVenda.HasValue);
    }
}
