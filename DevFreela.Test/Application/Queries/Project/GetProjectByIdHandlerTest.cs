using DevFreela.API.DTOs;
using DevFreela.Application.Queries.Project.GetProjectById;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.CacheStorage;
using Moq;

namespace DevFreela.Test.Application.Queries.Project;

public class GetProjectByIdHandlerTest
{
    [Fact]
    public async Task Handle_ProjectFoundInCache_ReturnsProjectFromCache()
    {
        //Arrange 
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        var cachedProject = new ProjectViewModel(
            1,
            "Project 1",
            "Project description",
            1,
            2,
            "Client Name",
            "Freelancer Name",
            10000,
            new List<string> { "Comment 1" }
        );

        cacheServiceMock.Setup(x => x.GetAsync<ProjectViewModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedProject);
        
        var handler = new GetProjectByIdHandler(projectRepositoryMock.Object, cacheServiceMock.Object);
        var request = new GetProjectByIdQuery(1); 
        
        //Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(cachedProject.Title, result.Data.Title);
        projectRepositoryMock.Verify(x => x.GetDetailsById(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ProjectNotFoundInCacheAndRepository_ReturnsError()
    {
        //Arrange 
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        
        cacheServiceMock.Setup(x => x.GetAsync<ProjectViewModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProjectViewModel)null);

        projectRepositoryMock.Setup(x => x.GetDetailsById(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Project)null);
        
        var handler = new GetProjectByIdHandler(projectRepositoryMock.Object, cacheServiceMock.Object);
        var request = new GetProjectByIdQuery(1); 
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(x => x.GetDetailsById(It.IsAny<int>()), Times.Once);

    }
    
    [Fact]
    public async Task Handle_ProjectNotInCacheButFoundInRepository_ReturnsProjectAndStoresInCache()
    {
        // Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var cacheServiceMock = new Mock<ICacheService>();
        
        var project = new Domain.Entities.Project("Project title", "Project description", 1, 2, 10000);
        var client = new Domain.Entities.User("Client", "client@example.com", "123456a", DateTime.Now);
        var freelancer = new Domain.Entities.User("Freelancer", "freelancer@example.com", "123456a", DateTime.Now);
        
        project.AssignUsers(client, freelancer);
        
        projectRepositoryMock.Setup(x => x.GetDetailsById(It.IsAny<int>())).ReturnsAsync(project);
        cacheServiceMock.Setup(x => x.GetAsync<ProjectViewModel>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((ProjectViewModel)null);
        
        var handler = new GetProjectByIdHandler(projectRepositoryMock.Object, cacheServiceMock.Object);
        var request = new GetProjectByIdQuery(project.Id);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Project title", result.Data.Title);
        projectRepositoryMock.Verify(x => x.GetDetailsById(It.IsAny<int>()), Times.Once); // Verifica se o repositório foi chamado uma vez
        cacheServiceMock.Verify(
            x => x.SetAsync(
                It.IsAny<string>(),                         
                It.IsAny<ProjectViewModel>(),               
                It.IsAny<int>(),                            
                It.IsAny<int>(),                            
                It.IsAny<CancellationToken>()              
            ), 
            Times.Once
        );
    }
}