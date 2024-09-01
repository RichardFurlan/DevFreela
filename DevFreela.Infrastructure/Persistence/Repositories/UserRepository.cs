using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DevFreelaDbContext context) : base(context)
    {
    }

    public async Task<User?> GetDetailsById(int id)
    {
        return await _context.Users
            .Include(u => u.Skills)
            .ThenInclude(u => u.Skill)
            .Where(p => !p.IsDeleted)
            .SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddUserSkill(UserSkill userSkill)
    {
        await _context.UserSkills.AddAsync(userSkill);
        await _context.SaveChangesAsync();
    }
}