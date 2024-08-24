using DevFreela.API.Entities;

namespace DevFreela.API.DTOs
{
    public record ProjectViewModel(
        int Id,
        string Title,
        string Description,
        int IdClient,
        int IdFreelancer,
        string ClienteName,
        string FreelancerName,
        decimal TotalCost,
        List<string> Comments)
    {
        public static ProjectViewModel FromEntity(Project entity)
            => new ProjectViewModel(
                entity.Id, 
                entity.Title, 
                entity.Description, 
                entity.IdClient, 
                entity.IdFreelancer,
                entity.Client.FullName, 
                entity.Freelancer.FullName, 
                entity.TotalCost, 
                entity.Comments.Select(c => c.Content).ToList());
    }
}
