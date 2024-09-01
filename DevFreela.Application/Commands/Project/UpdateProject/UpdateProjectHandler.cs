using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Project.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _projectRepository;
    public UpdateProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }


    public async Task<ResultViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.IdProject);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
            
        project.Update(request.Title, request.Description, request.TotalCost);

        await _projectRepository.UpdateAsync(project);
        
        return ResultViewModel.Success();
    }
}