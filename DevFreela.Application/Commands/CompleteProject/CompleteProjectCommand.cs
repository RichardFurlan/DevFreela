using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.CompleteProject;

public record CompleteProjectCommand(int Id) : IRequest<ResultViewModel>;