using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<List<Skill>> GetAll(string search, int page, int size);
}