using DevFreela.Application.DTOs;
using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Project.InsertProject;

public class InserProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<int>>
{
    private readonly IMediator _mediator;
    private readonly IProjectRepository _projectRepository;

    public InserProjectHandler(IMediator mediator, IProjectRepository projectRepository)
    {
        _mediator = mediator;
        _projectRepository = projectRepository;
    }

    public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
    {
        var project = request.ToEntity();

        await _projectRepository.Add(project);

        var projectCreated = new ProjectCreatedNotification(project.Id, project.Title, project.TotalCost);
        await _mediator.Publish(projectCreated);
        
        return ResultViewModel<int>.Success(project.Id);
    }
}