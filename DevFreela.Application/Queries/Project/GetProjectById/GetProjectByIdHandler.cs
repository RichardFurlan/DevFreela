using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.CacheStorage;
using MediatR;

namespace DevFreela.Application.Queries.Project.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ResultViewModel<ProjectViewModel>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cacheService;
    public GetProjectByIdHandler(IProjectRepository projectRepository, ICacheService cacheService)
    {
        _projectRepository = projectRepository;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<ProjectViewModel>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = request.Id.ToString();
        var cacheObject = await _cacheService.GetAsync<ProjectViewModel>(cacheKey, cancellationToken);
        if (cacheObject != null)
        {
            return ResultViewModel<ProjectViewModel>.Success(cacheObject);
        }
        var project = await _projectRepository.GetDetailsById(request.Id);
        
        if (project is null)
        {
            return ResultViewModel<ProjectViewModel>.Error("Projeto não encontrado");
        }
        
        var model = ProjectViewModel.FromEntity(project);
        
        await _cacheService.SetAsync(cacheKey, model, cancellationToken: cancellationToken);
        
        return ResultViewModel<ProjectViewModel>.Success(model);
    }
}