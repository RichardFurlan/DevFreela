using DevFreela.Application.Commands.User.InsertUser;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Services.AuthService;
using Moq;

namespace DevFreela.Test.Application.Commands.User;

public class InsertUserHandlerTest
{
    [Fact]
    public async Task Handle_ValidPassword_ShouldAddUserAndReturnSuccess()
    {
        // Arrange 
        var userRepositoryMock = new Mock<IUserRepository>();
        var authServiceMock = new Mock<IAuthService>();

        authServiceMock
            .Setup(auth => auth.ValidarSenha(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string)null);

        authServiceMock
            .Setup(auth => auth.ComputeSha256Hash(It.IsAny<string>()))
            .Returns("hashedPassword");

        userRepositoryMock
            .Setup(ur => ur.AddAsync(It.IsAny<Domain.Entities.User>()))
            .ReturnsAsync(1);

        var handler = new InsertUserHandler(userRepositoryMock.Object, authServiceMock.Object);
        var command = new InsertUserCommand("Richard", "richard@gmail.com", "password123", "password123",
            new DateTime(1990, 1, 1));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<Domain.Entities.User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ShouldReturnError()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var authServiceMock = new Mock<IAuthService>();

        authServiceMock
            .Setup(auth => auth.ValidarSenha(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("As senhas não são idênticas");

        
        var handler = new InsertUserHandler(userRepositoryMock.Object, authServiceMock.Object);
        var command = new InsertUserCommand("Richard", "richard@example.com", "password123", "password321", new DateTime(1990, 1, 1));
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("As senhas não são idênticas", result.Message);
        userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<Domain.Entities.User>()), Times.Never);
    }
}