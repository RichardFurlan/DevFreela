using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.CompleteProject;

public record CompleteProjectCommand(int Id) : IRequest<ResultViewModel>;