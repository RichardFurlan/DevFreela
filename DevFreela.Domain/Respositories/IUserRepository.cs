using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetDetailsById(int id);
    Task AddUserSkill(UserSkill userSkill);
}