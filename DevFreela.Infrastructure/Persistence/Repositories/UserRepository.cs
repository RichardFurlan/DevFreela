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
    
    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
        return await _context
            .Users
            .SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
    }

    public async Task AddUserSkill(UserSkill userSkill)
    {
        await _context.UserSkills.AddAsync(userSkill);
        await _context.SaveChangesAsync();
    }
}