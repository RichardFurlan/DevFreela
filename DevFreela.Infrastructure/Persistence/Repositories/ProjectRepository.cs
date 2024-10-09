using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IGenericRepository<Project> _genericRepository;
    private readonly DevFreelaDbContext _context;

    public ProjectRepository(IGenericRepository<Project> genericRepository, DevFreelaDbContext context)
    {
        _genericRepository = genericRepository;
        _context = context;
    }

    public async Task<int> AddAsync(Project project)
    {
        var projectId = await _genericRepository.AddAsync(project);
        return projectId;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var project = await _genericRepository.GetByIdAsync(id);
        return project;
    }

    public Task UpdateAsync(Project project)
    {
        _genericRepository.UpdateAsync(project);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _genericRepository.ExistsAsync(id);
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
        await _genericRepository.SaveAsync();
    }
}