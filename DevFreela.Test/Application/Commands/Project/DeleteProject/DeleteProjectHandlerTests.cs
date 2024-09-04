using DevFreela.Application.Commands.Project.DeleteProject;
using DevFreela.Domain.Enums;
using DevFreela.Domain.Respositories;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.DeleteProject;

public class DeleteProjectHandlerTests
{
    [Fact]
    public async Task Handle_ProjectExists_ReturnsSuccess()
    {
        //Arrange
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descricao do projeto", 1, 2, 10000);

        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new DeleteProjectHandler(projectRepositoryMock.Object);
        var command = new DeleteProjectCommand(1);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.Is<Domain.Entities.Project>(p => p.IsDeleted)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ProjectDoesNotExist_ReturnsError()
    {
        //Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Project)null);

        var handler = new DeleteProjectHandler(projectRepositoryMock.Object);
        var command = new DeleteProjectCommand(1);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
}