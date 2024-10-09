using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly IGenericRepository<Skill> _genericRepository;
    private readonly DevFreelaDbContext _context;
    public SkillRepository(DevFreelaDbContext context, IGenericRepository<Skill> genericRepository)
    {
        _context = context;
        _genericRepository = genericRepository;
    }

    public async Task<int> AddAsync(Skill skill)
    {
        var skillId = await _genericRepository.AddAsync(skill);
        return skillId;
    }

    public async Task<List<Skill>> GetAll(string search, int page, int size)
    {
        return await _context.Skills
            .Where(p => !p.IsDeleted && (search == "" || p.Description.Contains(search)))
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }
}