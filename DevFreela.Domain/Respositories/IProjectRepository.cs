using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<List<Project>> GetAll(string search, int page, int size);
    Task<Project?> GetDetailsById(int id);
    Task AddComment(ProjectComment comment);
}