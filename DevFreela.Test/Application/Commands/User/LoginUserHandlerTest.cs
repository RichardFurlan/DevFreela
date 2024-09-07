using DevFreela.Application.Commands.User.LoginUser;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Services.AuthService;
using Moq;

namespace DevFreela.Test.Application.Commands.User;

public class LoginUserHandlerTest
{
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var authServiceMock = new Mock<IAuthService>();
        var handler = new LoginUserHandler(authServiceMock.Object, userRepositoryMock.Object);
        var cancellationToken = new CancellationToken();

        var command = new LoginUserCommand("test@exemplo.com", "password123");

        var hashedPassword = "hashed_password123";
        var user = new Domain.Entities.User("Teste", command.Email, hashedPassword, new DateTime(1990, 08, 08 ));

        authServiceMock.Setup(x => x.ComputeSha256Hash(command.Password)).Returns(hashedPassword);
        userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(command.Email, hashedPassword)).ReturnsAsync(user);
        authServiceMock.Setup(x => x.GenerateJwtToken(user.Email)).Returns("token123");

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("token123", result.Data.token);
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsError()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var authServiceMock = new Mock<IAuthService>();
        var handler = new LoginUserHandler(authServiceMock.Object, userRepositoryMock.Object);
        var cancellationToken = new CancellationToken();

        var command = new LoginUserCommand("test@exemplo.com", "SenhaErrada");

        var hashedPassword = "hashed_wrongpassword";

        authServiceMock.Setup(x => x.ComputeSha256Hash(command.Password)).Returns(hashedPassword);
        userRepositoryMock.Setup(x => x.GetUserByEmailAndPasswordAsync(command.Email, hashedPassword)).ReturnsAsync((Domain.Entities.User)null);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("E-mail ou senha incorretos, tente novamente.", result.Message);
    }
}