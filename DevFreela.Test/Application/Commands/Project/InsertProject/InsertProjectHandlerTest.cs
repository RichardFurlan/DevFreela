using DevFreela.Application.Commands.Project.InsertProject;
using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Domain.Respositories;
using MediatR;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.InsertProject;

public class InsertProjectHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var projectRepositoryMock = new Mock<IProjectRepository>();
        
        var insertProjectCommand = new InsertProjectCommand("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
        
        var project = new Domain.Entities.Project(insertProjectCommand.Title, insertProjectCommand.Description, insertProjectCommand.IdClient, insertProjectCommand.IdFreelancer, insertProjectCommand.TotalCost);
        projectRepositoryMock
            .Setup(pr => pr.AddAsync(It.IsAny<Domain.Entities.Project>()))
            .ReturnsAsync(project.Id);
        
        mediatorMock
            .Setup(m => m.Publish(It.IsAny<ProjectCreatedNotification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new InserProjectHandler(mediatorMock.Object, projectRepositoryMock.Object);
        
        // Act
        var result = await handler.Handle(insertProjectCommand, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Nome do projeto", insertProjectCommand.Title);
        projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Domain.Entities.Project>()), Times.Once);
        mediatorMock.Verify(m => m.Publish(It.IsAny<ProjectCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}