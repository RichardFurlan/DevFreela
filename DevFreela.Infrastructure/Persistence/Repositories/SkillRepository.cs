using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(DevFreelaDbContext context) : base(context)
    {
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