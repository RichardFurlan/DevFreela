using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task<int> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task SaveAsync();

}