using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Queries.Project.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, ResultViewModel<List<ProjectItemViewModel>>>
{
    private readonly IProjectRepository _projectRepository;
    public GetAllProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<ResultViewModel<List<ProjectItemViewModel>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAll(request.Search, request.Page, request.Size);

        var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();

        return ResultViewModel<List<ProjectItemViewModel>>.Success(model);
    }
}