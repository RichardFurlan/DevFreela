using DevFreela.Application.Commands.Project.CompleteProject;
using DevFreela.Application.Commands.Project.StartProject;
using DevFreela.Domain.Enums;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.StartProject;

public class StartProjectHandlerTest
{
    [Fact]
    public async Task Handle_ProjectExists_ProjectStartedSuccessfully()
    {
        //Arrange
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descricao do projeto", 1, 2, 10000);

        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new StartProjectHandler(projectRepositoryMock.Object);
        var command = new StartProjectCommand(1);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EnumProjectStatus.InProgress, projectMock.Status);
        Assert.NotNull(projectMock.StartedAt);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectDoesNotExist_ReturnsError()
    {
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Project)null);

        var handler = new StartProjectHandler(projectRepositoryMock.Object);
        var command = new StartProjectCommand(1);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ProjectExistsButCannotBeStarted_ReturnsError()
    {
        // Arrange
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
        projectMock.Start(); 
        projectMock.Cancel();

        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new StartProjectHandler(projectRepositoryMock.Object);
        var command = new StartProjectCommand(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("O projeto não pode ser iniciado. Apenas projetos com a situação criado", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
}