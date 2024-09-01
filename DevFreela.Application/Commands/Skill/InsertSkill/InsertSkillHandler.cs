using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Commands.Skill.InsertSkill;

public class InsertSkillHandler : IRequestHandler<InsertSkillCommand, ResultViewModel<int>>
{
    private readonly ISkillRepository _skillRepository;
    public InsertSkillHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }
    public async Task<ResultViewModel<int>> Handle(InsertSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = request.ToEntity();

        await _skillRepository.AddAsync(skill);

        return ResultViewModel<int>.Success(skill.Id);
    }
}