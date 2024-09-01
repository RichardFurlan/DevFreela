using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.StartProject;

public record StartProjectCommand(int Id) : IRequest<ResultViewModel>;