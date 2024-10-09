using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DevFreelaDbContext _context;
    private readonly GenericRepository<User> _genericRepository;
    public UserRepository(DevFreelaDbContext context, GenericRepository<User> genericRepository)
    {
        _context = context;
        _genericRepository = genericRepository;
    }

    public async Task<User?> GetDetailsById(int id)
    {
        return await _context.Users
            .Include(u => u.Skills)
            .ThenInclude(u => u.Skill)
            .Where(p => !p.IsDeleted)
            .SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _genericRepository.GetByIdAsync(id);
        return user;
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
        await _genericRepository.SaveAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _genericRepository.ExistsAsync(id);
    }

    public async Task UpdateAsync(User user)
    {
        await _genericRepository.UpdateAsync(user);
    }

    public async Task<int> AddAsync(User user)
    {
        var userId = await _genericRepository.AddAsync(user);
        return userId;
    }
}