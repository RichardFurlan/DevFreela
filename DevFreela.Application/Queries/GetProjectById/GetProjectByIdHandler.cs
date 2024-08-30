using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ResultViewModel<ProjectViewModel>>
{
    private readonly IProjectRepository _projectRepository;
    public GetProjectByIdHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ResultViewModel<ProjectViewModel>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetDetailsById(request.Id);
        
        if (project is null)
        {
            return ResultViewModel<ProjectViewModel>.Error("Projeto não encontrado");
        }
        
        var model = ProjectViewModel.FromEntity(project);
        
        return ResultViewModel<ProjectViewModel>.Success(model);
    }
}