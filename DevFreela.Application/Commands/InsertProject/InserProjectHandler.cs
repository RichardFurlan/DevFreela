using DevFreela.Application.DTOs;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject;

public class InserProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<int>>
{
    private readonly DevFreelaDbContext _context;
    public InserProjectHandler(DevFreelaDbContext context)
    {
        _context = context;
    }

    public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
    {
        var project = request.ToEntity();
        
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        
        return ResultViewModel<int>.Success(project.Id);
    }
}