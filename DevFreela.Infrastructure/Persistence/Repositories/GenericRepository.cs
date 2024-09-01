using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly DevFreelaDbContext _context;
    protected DbSet<T> Entity => _context.Set<T>();
    public GenericRepository(DevFreelaDbContext context)
    {
        _context = context;
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await Entity.SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Entity.ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await Entity.AnyAsync(e => e.Id == id);;
    }

    public async Task<int> AddAsync(T entity)
    {
        await Entity.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        Entity.Update(entity);
        await _context.SaveChangesAsync();
    }
}