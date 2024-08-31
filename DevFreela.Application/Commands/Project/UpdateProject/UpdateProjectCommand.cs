using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.UpdateProject;

public record UpdateProjectCommand(int IdProject, string Title, string Description, decimal TotalCost) : IRequest<ResultViewModel>;