using DevFreela.Application.DTOs;
using DevFreela.Domain.Respositories;
using MediatR;

namespace DevFreela.Application.Queries.Skill.GetAllSkills;

public class GetAllSkillsHandler : IRequestHandler<GetAllSkillsQuery, ResultViewModel<List<SkillItemViewModel>>>
{
    private readonly ISkillRepository _skillRepository;
    public GetAllSkillsHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }
    public async Task<ResultViewModel<List<SkillItemViewModel>>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.GetAll(request.Search, request.Page, request.Size);

        var model = skills.Select(SkillItemViewModel.FromEntity).ToList();

        return ResultViewModel<List<SkillItemViewModel>>.Success(model);
    }
}