using DevFreela.Domain.Entities;

namespace DevFreela.Domain.Respositories;

public interface ISkillRepository
{
    Task<int> AddAsync(Skill skill);
    Task<List<Skill>> GetAll(string search, int page, int size);
}