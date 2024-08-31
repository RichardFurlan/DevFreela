using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Project.InsertProject;

public record InsertProjectCommand(string Title, string Description, int IdCliente, int IdFreelancer, decimal TotalCost) : IRequest<ResultViewModel<int>>
{
    public Domain.Entities.Project ToEntity()
        => new(Title, Description, IdCliente, IdFreelancer, TotalCost);
};