using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface IProjectRepository
{
    Task<int> AddAsync(Project project);
    Task<Project?> GetByIdAsync(int id);
    Task UpdateAsync(Project project);
    Task<bool> ExistsAsync(int id);
    Task<List<Project>> GetAll(string search, int page, int size);
    Task<Project?> GetDetailsById(int id);
    Task AddComment(ProjectComment comment);
}