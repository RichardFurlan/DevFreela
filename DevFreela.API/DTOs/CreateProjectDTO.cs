namespace DevFreela.API.DTOs;

public record CreateProjectDTO(string Title, string Description, int IdCliente, int IdFreelancer, decimal TotalCost)
{
    
};