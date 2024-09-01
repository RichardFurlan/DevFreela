using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Queries.Project.GetAllProjects;

public record GetAllProjectsQuery(string Search, int Page, int Size) : IRequest<ResultViewModel<List<ProjectItemViewModel>>>;
