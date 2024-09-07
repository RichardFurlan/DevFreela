using DevFreela.Application.Commands.User.InsertUserSkill;
using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Commands.User;

public class InsertUserSkillHandlerTest
{
    [Fact]
    public async Task Handle_UserExists_AddSkills()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var handler = new InsertUserSkillHandler(userRepositoryMock.Object);
        var cancellationToken = new CancellationToken();

        int[] SkillIds = [1, 2];
        var command = new InsertUserSkillCommand(1, SkillIds);

        userRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        userRepositoryMock.Verify(x => x.AddUserSkill(It.IsAny<UserSkill>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnError()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var handler = new InsertUserSkillHandler(userRepositoryMock.Object);
        var cancellationToken = new CancellationToken();

        int[] SkillIds = [1, 2];
        var command = new InsertUserSkillCommand(1, SkillIds);

        userRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Usuário não encontrado", result.Message);
        userRepositoryMock.Verify(x => x.AddUserSkill(It.IsAny<UserSkill>()), Times.Never);
    }
}