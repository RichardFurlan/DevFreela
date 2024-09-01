using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Queries.Project.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<ResultViewModel<ProjectViewModel>>;