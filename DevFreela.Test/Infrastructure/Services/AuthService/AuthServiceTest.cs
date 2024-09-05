using Microsoft.Extensions.Configuration;
using Moq;

namespace DevFreela.Test.Infrastructure.Services.AuthService;

public class AuthServiceTest
{
    private readonly DevFreela.Infrastructure.Services.AuthService.AuthService _authService;

    public AuthServiceTest()
    {
        var configurationMock = new Mock<IConfiguration>();
        _authService = new DevFreela.Infrastructure.Services.AuthService.AuthService(configurationMock.Object);
    }
    
     [Fact]
    public void ValidarSenha_PasswordsNotMatching_ShouldReturnError()
    {
        // Arrange
        var password = "senha123";
        var passwordConfirm = "password456";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Equal("As senhas não são idênticas", result);
    }

    [Fact]
    public void ValidarSenha_PasswordEmpty_ShouldReturnError()
    {
        // Arrange
        var password = "";
        var passwordConfirm = "";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Equal("A senha não pode estar em branco.", result);
    }

    [Fact]
    public void ValidarSenha_PasswordTooShort_ShouldReturnError()
    {
        // Arrange
        var password = "abc";
        var passwordConfirm = "abc";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Equal("A senha deve ter pelo menos 6 caracteres.", result);
    }

    [Fact]
    public void ValidarSenha_PasswordWithoutNumber_ShouldReturnError()
    {
        // Arrange
        var password = "abcdef";
        var passwordConfirm = "abcdef";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Equal("A senha deve conter pelo menos um número", result);
    }

    [Fact]
    public void ValidarSenha_PasswordWithoutLetter_ShouldReturnError()
    {
        // Arrange
        var password = "123456";
        var passwordConfirm = "123456";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Equal("A senha deve conter pelo menos uma letra.", result);
    }

    [Fact]
    public void ValidarSenha_ValidPassword_ShouldReturnNull()
    {
        // Arrange
        var password = "senha123";
        var passwordConfirm = "senha123";

        // Act
        var result = _authService.ValidarSenha(password, passwordConfirm);

        // Assert
        Assert.Null(result); // Nenhum erro, validação passou
    }
}