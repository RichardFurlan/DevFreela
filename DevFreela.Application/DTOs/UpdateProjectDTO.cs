namespace DevFreela.API.DTOs;

public record UpdateProjectDTO(int IdProject, string Title, string Description, decimal TotalCost);