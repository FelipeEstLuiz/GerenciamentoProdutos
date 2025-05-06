using Application.Core.Command.Produto;
using Application.Core.Validators.Produto;
using Application.Domain.Enums;
using FluentValidation.TestHelper;

namespace Tests.Application.Core.Validators.Produto;

public class UpdateProdutoValidatorTests
{
    private readonly UpdateProdutoValidator _updateProdutoValidator;

    public UpdateProdutoValidatorTests() => _updateProdutoValidator = new UpdateProdutoValidator();

    [Fact]
    public void Deve_Falhar_Se_Id_Eh_Invalido()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 0
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Invalido");
    }

    [Fact]
    public void Deve_Falhar_Se_Nome_Eh_Nulo_Ou_Vazio()
    {
        // Arrange
        UpdateProdutoCommand command = new(null, "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Obrigatorio");
    }

    [Fact]
    public void Deve_Falhar_Se_Nome_Tem_Tamanho_Invalido()
    {
        // Arrange
        UpdateProdutoCommand command = new("A", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Deve ter entre 5 e 250 caracter");
    }

    [Fact]
    public void Deve_Passar_Se_Nome_Eh_Valido()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Falhar_Se_Descricao_Tem_Tamanho_Invalido()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "A", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Descricao).WithErrorMessage("Deve ter entre 5 e 800 caracter");
    }

    [Fact]
    public void Deve_Passar_Se_Descricao_Eh_Valida()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao Valida", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Deve_Falhar_Se_Valor_Eh_Nulo_Ou_Menor_Igual_A_Zero()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 0, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Valor).WithErrorMessage("Deve ser maior que 0");
    }

    [Fact]
    public void Deve_Passar_Se_Valor_Eh_Valido()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow)
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Valor);
    }

    [Fact]
    public void Deve_Falhar_Se_DataUltimaVenda_Eh_Futura()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow.AddDays(1))
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DataUltimaVenda).WithErrorMessage("Nao pode ser posterior a data atual");
    }

    [Fact]
    public void Deve_Passar_Se_DataUltimaVenda_Eh_Valida()
    {
        // Arrange
        UpdateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1, StatusProduto.Disponivel, DateTime.UtcNow.AddDays(-1))
        {
            Id = 1
        };

        // Act
        TestValidationResult<UpdateProdutoCommand> result = _updateProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DataUltimaVenda);
    }
}
