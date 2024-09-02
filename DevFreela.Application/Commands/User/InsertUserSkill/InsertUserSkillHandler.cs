using DevFreela.Application.DTOs;
using DevFreela.Domain.Entities;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUserSkill;

public class InsertUserSkillHandler : IRequestHandler<InsertUserSkillCommand, ResultViewModel>
{
    private readonly IUserRepository _userRepository;
    public InsertUserSkillHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ResultViewModel> Handle(InsertUserSkillCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.ExistsAsync(request.IdUser);

        if (!userExists)
        {
            return ResultViewModel.Error("Usuário não encontrado");
        }

        foreach (var skillId in request.SkillIds)
        {
            var userSkill = new UserSkill(request.IdUser, skillId);
            await _userRepository.AddUserSkill(userSkill);
        };
        
        return ResultViewModel.Success();
    }
}