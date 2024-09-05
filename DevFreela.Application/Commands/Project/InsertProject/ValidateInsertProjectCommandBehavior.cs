using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.Project.InsertProject;

public class ValidateInsertProjectCommandBehavior : IPipelineBehavior<InsertProjectCommand, ResultViewModel<int>>
{
    private readonly IUserRepository _userRepository;
    public ValidateInsertProjectCommandBehavior(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, RequestHandlerDelegate<ResultViewModel<int>> next, CancellationToken cancellationToken)
    {
        var clientExists = await _userRepository.ExistsAsync(request.IdClient);
        var freelancerExists = await _userRepository.ExistsAsync(request.IdFreelancer);

        if (!clientExists || !freelancerExists)
        {
            return ResultViewModel<int>.Error("Cliente ou Freelancer inválidos");
        }
        
        return await next();
    }
}