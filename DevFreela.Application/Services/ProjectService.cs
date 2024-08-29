using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;
using DevFreela.Domain.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services;

public class ProjectService : IProjectService
{
    private readonly DevFreelaDbContext _context;
    public ProjectService(DevFreelaDbContext context)
    {
        _context = context;
    }
    public ResultViewModel<List<ProjectItemViewModel>> GetAll(string search, int page, int size)
    {
        var projects = _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
            .Skip(page * size)
            .Take(size)
            .ToList();

        var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();

        return ResultViewModel<List<ProjectItemViewModel>>.Success(model);
    }

    public ResultViewModel<ProjectViewModel> GetById(int id)
    {
        var project = _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Include(p => p.Comments)
            .Where(p => !p.IsDeleted)
            .SingleOrDefault(p => p.Id == id);
        
        if (project is null)
        {
            return ResultViewModel<ProjectViewModel>.Error("Projeto não encontrado");
        }
        
        var model = ProjectViewModel.FromEntity(project);
        
        return ResultViewModel<ProjectViewModel>.Success(model);
    }

    public ResultViewModel<int> Insert(CreateProjectDTO model)
    {
        var project = model.ToEntity();
        
        _context.Projects.Add(project);
        _context.SaveChanges();
        
        return ResultViewModel<int>.Success(project.Id);
    }

    public ResultViewModel Update(int id, UpdateProjectDTO model)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
            
        project.Update(model.Title, model.Description, model.TotalCost);
        
        _context.Projects.Update(project);
        _context.SaveChanges();
        
        return ResultViewModel.Success();
    }

    public ResultViewModel Delete(int id)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        project.SetAsDeleted();
        
        _context.Projects.Update(project);
        _context.SaveChanges();

        return ResultViewModel.Success();
    }

    public ResultViewModel Start(int id)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        project.Start();
        
        _context.Projects.Update(project);
        _context.SaveChanges();

        return ResultViewModel.Success();
    }

    public ResultViewModel Complete(int id)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }
        
        project.Complete();
        
        _context.Projects.Update(project);
        _context.SaveChanges();

        return ResultViewModel.Success();
    }

    public ResultViewModel InsertComment(int id, CreateProjectCommentDTO model)
    {
        var project = _context.Projects.SingleOrDefault(p => p.Id == id);

        if (project is null)
        {
            return ResultViewModel.Error("Projeto não encontrado");
        }

        var comment = new ProjectComment(model.Content, model.IdProject, model.IdUser);
        _context.ProjectComments.Add(comment);
        _context.SaveChanges();
        
        return ResultViewModel.Success();
    }
}