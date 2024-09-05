using DevFreela.Application.Commands.Skill.InsertSkill;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Commands.Skill;

public class InsertSkillHandlerTest
{
    [Fact]
    public async Task Handle_ValidSkill_ShouldSkillAndReturnSuccess()
    {
        // Arrange 
        var skillRepositoryMock = new Mock<ISkillRepository>();
        skillRepositoryMock
            .Setup(sr => sr.AddAsync(It.IsAny<Domain.Entities.Skill>()))
            .ReturnsAsync(1);

        var handler = new InsertSkillHandler(skillRepositoryMock.Object);
        var command = new InsertSkillCommand("C#");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        skillRepositoryMock.Verify(sr => sr.AddAsync(It.IsAny<Domain.Entities.Skill>()), Times.Once);
    }
}