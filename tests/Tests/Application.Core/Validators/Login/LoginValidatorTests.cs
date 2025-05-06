using Application.Core.Command.Login;
using Application.Core.Validators.Login;
using FluentValidation.TestHelper;

namespace Tests.Application.Core.Validators.Login;

public class LoginValidatorTests
{
    private readonly LoginValidator _loginValidator;

    public LoginValidatorTests() => _loginValidator = new LoginValidator();

    [Fact]
    public void Deve_Falhar_Se_Email_Eh_Nulo_Ou_Vazio()
    {
        // Arrange
        LoginCommand command = new(null, "Senha123");

        // Act
        TestValidationResult<LoginCommand> result = _loginValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Obrigatorio");
    }

    [Fact]
    public void Deve_Falhar_Se_Email_Eh_Invalido()
    {
        // Arrange
        LoginCommand command = new("email_invalido", "Senha123");

        // Act
        TestValidationResult<LoginCommand> result = _loginValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Invalido");
    }

    [Fact]
    public void Deve_Passar_Se_Email_Eh_Valido()
    {
        // Arrange
        LoginCommand command = new("email@teste.com", "Senha123");

        // Act
        TestValidationResult<LoginCommand> result = _loginValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Deve_Falhar_Se_Senha_Eh_Nula_Ou_Vazia()
    {
        // Arrange
        LoginCommand command = new("email@teste.com", null);

        // Act
        TestValidationResult<LoginCommand> result = _loginValidator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Senha).WithErrorMessage("Obrigatorio");
    }

    [Fact]
    public void Deve_Passar_Se_Senha_Eh_Valida()
    {
        // Arrange
        LoginCommand command = new("email@teste.com", "Senha123");

        // Act
        TestValidationResult<LoginCommand> result = _loginValidator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Senha);
    }
}
