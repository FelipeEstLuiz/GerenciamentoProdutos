using Application.Core.Command.Produto;
using Application.Core.Validators.Produto;
using FluentValidation.TestHelper;

namespace Tests.Application.Core.Validators.Produto;

public class CreateProdutoValidatorTests
{
    private readonly CreateProdutoValidator _createProdutoValidator;

    public CreateProdutoValidatorTests()
    {
        _createProdutoValidator = new CreateProdutoValidator();
    }

    [Fact]
    public void Deve_Falhar_Se_Nome_Eh_Nulo_Ou_Vazio()
    {
        // Arrange
        CreateProdutoCommand command = new(null, "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Obrigatorio");
    }

    [Fact]
    public void Deve_Falhar_Se_Nome_Tem_Tamanho_Invalido()
    {
        // Arrange
        CreateProdutoCommand command = new("A", "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("Deve ter entre 5 e 250 caracter");
    }

    [Fact]
    public void Deve_Passar_Se_Nome_Eh_Valido()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Falhar_Se_Descricao_Tem_Tamanho_Invalido()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "A", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Descricao).WithErrorMessage("Deve ter entre 5 e 800 caracter");
    }

    [Fact]
    public void Deve_Passar_Se_Descricao_Eh_Valida()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao Valida", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Deve_Falhar_Se_Valor_Eh_Nulo_Ou_Menor_Igual_A_Zero()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 0, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Valor).WithErrorMessage("Deve ser maior que 0");
    }

    [Fact]
    public void Deve_Passar_Se_Valor_Eh_Valido()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Valor);
    }

    [Fact]
    public void Deve_Falhar_Se_QuantidadeEstoque_Eh_Nulo_Ou_Menor_Igual_A_Zero()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 0, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.QuantidadeEstoque).WithErrorMessage("Deve ser maior que 0");
    }

    [Fact]
    public void Deve_Passar_Se_QuantidadeEstoque_Eh_Valido()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.QuantidadeEstoque);
    }

    [Fact]
    public void Deve_Falhar_Se_Categoria_Eh_Nula()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, null);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categoria).WithErrorMessage("Obrigatorio");
    }

    [Fact]
    public void Deve_Passar_Se_Categoria_Eh_Valida()
    {
        // Arrange
        CreateProdutoCommand command = new("Produto Teste", "Descricao", 100.50m, 10, 1);

        // Act
        TestValidationResult<CreateProdutoCommand> result = _createProdutoValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Categoria);
    }
}
