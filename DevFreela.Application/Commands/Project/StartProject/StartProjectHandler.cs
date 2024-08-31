using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Project.StartProject;

public class StartProjectHandler : IRequestHandler<StartProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _projectRepository;
    public StartProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ResultViewModel> Handle(StartProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetById(request.Id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        project.Start();
        
        await _projectRepository.Update(project);

        return ResultViewModel.Success();
    }
}