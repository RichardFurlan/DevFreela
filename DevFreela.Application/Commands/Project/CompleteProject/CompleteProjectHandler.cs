using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Project.CompleteProject;

public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _projectRepository;
    public CompleteProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        var complete = project.Complete();
        if (!complete)
        {
            return ResultViewModel.Error("O projeto não pode ser completado. Apenas projetos com a situação andamento e com pagamento pendente podem ser completados");
        }

        await _projectRepository.UpdateAsync(project);

        return ResultViewModel.Success();
    }
}