using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface IUserRepository 
{
    Task<User?> GetDetailsById(int id);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
    Task AddUserSkill(UserSkill userSkill);
    Task<bool> ExistsAsync(int id);
    Task UpdateAsync(User user);
    Task<int> AddAsync(User user);
}