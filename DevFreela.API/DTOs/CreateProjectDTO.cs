using DevFreela.API.Entities;

namespace DevFreela.API.DTOs;

public record CreateProjectDTO(string Title, string Description, int IdCliente, int IdFreelancer, decimal TotalCost)
{
    public Project ToEntity()
        => new(Title, Description, IdCliente, IdFreelancer, TotalCost);
};