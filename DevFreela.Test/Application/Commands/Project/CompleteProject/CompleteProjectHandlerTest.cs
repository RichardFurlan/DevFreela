using DevFreela.Application.Commands.Project.CompleteProject;
using DevFreela.Application.Commands.Project.DeleteProject;
using DevFreela.Domain.Enums;
using DevFreela.Domain.Respositories;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.Services.PaymentService;
using Moq;

namespace DevFreela.Test.Application.Commands.Project.CompleteProject;

public class CompleteProjectHandlerTest
{
    [Fact]
    public async Task Handle_ProjectExistsAndIsInProgress_ReturnsSuccess()
    {
        //Arrange
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descricao do projeto", 1, 2, 10000);
        projectMock.Start();

        var paymentServiceMock = new Mock<IPaymentService>();
        paymentServiceMock
            .Setup(ps => ps.ProcessPaymentAsync(It.IsAny<PaymentInfoDTO>()))
            .Returns(Task.CompletedTask);
        
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new CompleteProjectHandler(projectRepositoryMock.Object, paymentServiceMock.Object);
        var command = new CompleteProjectCommand(1, "1234567", "123", "12/28", "Teste", 10000);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EnumProjectStatus.PaymentPending, projectMock.Status);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.Is<Domain.Entities.Project>(p => p.Status == EnumProjectStatus.PaymentPending)), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ProjectDoesNotExist_ReturnsError()
    {
        //Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Project)null);

        var processPaymentMock = new Mock<IPaymentService>();
        var handler = new CompleteProjectHandler(projectRepositoryMock.Object, processPaymentMock.Object);
        var command = new CompleteProjectCommand(1, "1234567", "123", "12/28", "Teste", 10000);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Projeto não encontrado", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ProjectExistsButCannotBeCompleted_ReturnsError()
    {
        //Arrange
        var projectMock = new Domain.Entities.Project("Nome do projeto", "Descricao do projeto", 1, 2, 10000);

        var processPaymentMock = new Mock<IPaymentService>();
        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(projectMock);

        var handler = new CompleteProjectHandler(projectRepositoryMock.Object, processPaymentMock.Object);
        var command = new CompleteProjectCommand(1, "1234567", "123", "12/28", "Teste", 10000);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(EnumProjectStatus.Created, projectMock.Status);
        Assert.Equal("O projeto não pode ser completado. Apenas projetos com a situação andamento e com pagamento pendente podem ser completados", result.Message);
        projectRepositoryMock.Verify(pr => pr.UpdateAsync(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }
}