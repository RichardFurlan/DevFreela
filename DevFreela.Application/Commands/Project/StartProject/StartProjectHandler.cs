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
        var project = await _projectRepository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        var started = project.Start();
        if (!started)
        {
            return ResultViewModel.Error("O projeto não pode ser iniciado. Apenas projetos com a situação criado");
        }
        
        await _projectRepository.UpdateAsync(project);

        return ResultViewModel.Success();
    }
}