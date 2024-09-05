using DevFreela.Application.Commands.Project.InsertComment;
using DevFreela.Application.Commands.Project.InsertProject;
using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.InsertComment;

public class InsertCommentHandlerTest
{
    [Fact]
    public async Task Handle_ProjectExists_ShouldAddCommentAndReturnSuccess()
    {
        // Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        

        var handler = new InsertCommentHandler(projectRepositoryMock.Object);
        var insertCommentCommand = new InsertCommentCommand("Comentario", 1, 1); 
        
        // Act
        var result = await handler.Handle(insertCommentCommand, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        projectRepositoryMock.Verify(pr => pr.AddComment(It.IsAny<ProjectComment>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ProjectDoesNotExists_ShouldReturnsError()
    {
        // Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(false);
        

        var handler = new InsertCommentHandler(projectRepositoryMock.Object);
        var insertCommentCommand = new InsertCommentCommand("Comentario", 1, 1); 
        
        // Act
        var result = await handler.Handle(insertCommentCommand, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(pr => pr.AddComment(It.IsAny<ProjectComment>()), Times.Never);
    }
}