using DevFreela.Application.Commands.Project.UpdateProject;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.UpdateProject;

public class UpdateProjectHandlerTest
{
    [Fact]
    public async Task Handle_ProjectExists_ProjectUpdatedSuccessfully()
    {
        // Assert
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
            
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new UpdateProjectHandler(projectRepositoryMock.Object);
        var command = new UpdateProjectCommand(1, "Novo título", "Nova descrição", 20000);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Novo título", projectMock.Title);
        Assert.Equal("Nova descrição", projectMock.Description);
        Assert.Equal(20000, projectMock.TotalCost);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectDoesNotExist_ReturnsError()
    {
        // Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Project)null);

        var handler = new UpdateProjectHandler(projectRepositoryMock.Object);
        var command = new UpdateProjectCommand(1, "Novo título", "Nova descrição", 20000);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
}