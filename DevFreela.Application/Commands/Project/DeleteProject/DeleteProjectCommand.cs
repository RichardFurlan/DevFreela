using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.DeleteProject;

public record DeleteProjectCommand(int Id) : IRequest<ResultViewModel>;