using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(DevFreelaDbContext context) : base(context)
    {
    }
    public async Task<List<Project>> GetAll(string search, int page, int size)
    {
        return await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
            .Skip(page * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<Project?> GetDetailsById(int id)
    {
        return await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Include(p => p.Comments)
            .Where(p => !p.IsDeleted)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddComment(ProjectComment comment)
    {
        await _context.ProjectComments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }
}