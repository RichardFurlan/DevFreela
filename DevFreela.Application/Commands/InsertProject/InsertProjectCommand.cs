using DevFreela.Application.DTOs;
using DevFreela.Domain.Entities;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject;

public record InsertProjectCommand(string Title, string Description, int IdCliente, int IdFreelancer, decimal TotalCost) : IRequest<ResultViewModel<int>>
{
    public Project ToEntity()
        => new(Title, Description, IdCliente, IdFreelancer, TotalCost);
};