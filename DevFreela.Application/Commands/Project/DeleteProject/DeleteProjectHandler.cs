using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Project.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _projectRepository;
    public DeleteProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ResultViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        project.SetAsDeleted();

        await _projectRepository.UpdateAsync(project);

        return ResultViewModel.Success();
    }
}