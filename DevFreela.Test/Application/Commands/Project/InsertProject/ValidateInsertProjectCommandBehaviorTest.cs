using DevFreela.Application.Commands.Project.InsertProject;
using DevFreela.Application.DTOs;
using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.InsertProject;

public class ValidateInsertProjectCommandBehaviorTest
{
    [Fact]
    public async Task Handle_ValidClientAndFreelancer_ReturnsSuccess()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        
        userRepositoryMock.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
        userRepositoryMock.Setup(repo => repo.ExistsAsync(2)).ReturnsAsync(true);
        

        var insertProjectCommand = new InsertProjectCommand("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
        var behavior = new ValidateInsertProjectCommandBehavior(userRepositoryMock.Object);

        // Act
        var result = await behavior.Handle(insertProjectCommand, () => Task.FromResult(ResultViewModel<int>.Success(1)), CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_InvalidFreelancer_ReturnsError()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        
        userRepositoryMock.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);
        userRepositoryMock.Setup(repo => repo.ExistsAsync(2)).ReturnsAsync(false);  // Freelancer não existe

        var insertProjectCommand = new InsertProjectCommand("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
        var behavior = new ValidateInsertProjectCommandBehavior(userRepositoryMock.Object);

        // Act
        var result = await behavior.Handle(insertProjectCommand, () => Task.FromResult(ResultViewModel<int>.Success(1)), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cliente ou Freelancer inválidos", result.Message);
    }
    
    [Fact]
    public async Task Handle_InvalidClient_ReturnsError()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        
        userRepositoryMock.Setup(repo => repo.ExistsAsync(1)).ReturnsAsync(true);   // Client não existe
        userRepositoryMock.Setup(repo => repo.ExistsAsync(2)).ReturnsAsync(false);  

        var insertProjectCommand = new InsertProjectCommand("Nome do projeto", "Descrição do projeto", 1, 2, 10000);
        var behavior = new ValidateInsertProjectCommandBehavior(userRepositoryMock.Object);

        // Act
        var result = await behavior.Handle(insertProjectCommand, () => Task.FromResult(ResultViewModel<int>.Success(1)), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cliente ou Freelancer inválidos", result.Message);
    }
}