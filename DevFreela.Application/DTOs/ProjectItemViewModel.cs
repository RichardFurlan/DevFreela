using DevFreela.Domain.Entities;

namespace DevFreela.API.DTOs;

public record ProjectItemViewModel(int Id, string Title, string ClientName, string FreelancerName, decimal TotalCost)
{
    public static ProjectItemViewModel FromEntity(Project entity)
        => new(entity.Id, entity.Title, entity.Client.FullName, entity.Freelancer.FullName, entity.TotalCost);
};