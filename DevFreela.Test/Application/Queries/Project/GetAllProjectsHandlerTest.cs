using DevFreela.Application.Queries.Project.GetAllProjects;
using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Moq;

namespace DevFreela.Test.Application.Queries.Project;

public class GetAllProjectsHandlerTest
{
    [Fact]
    public async Task Handle_ReturnsProjectList()
    {
        // Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var client = new User("Teste", "fake@mail", "password123", new DateTime(1990, 08, 08));
        var freelancer = new User("Teste", "fake@mail", "password123", new DateTime(1990, 08, 08));

        var projects = new List<Domain.Entities.Project>
        {
            new Domain.Entities.Project("Project 1", "Projeto Teste", client.Id, freelancer.Id, 10000),
            new Domain.Entities.Project("Project 2", "Projeto Teste", client.Id, freelancer.Id, 10000)
        };
        
        projects[0].AssignUsers(client, freelancer);
        projects[1].AssignUsers(client, freelancer);
        
        projectRepositoryMock.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(projects);
        
        var handler = new GetAllProjectsHandler(projectRepositoryMock.Object);
        var request = new GetAllProjectsQuery(null, 1, 10);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Project 1", result.Data[0].Title);
    }
}