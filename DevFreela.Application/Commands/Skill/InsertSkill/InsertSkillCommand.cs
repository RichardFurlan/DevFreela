using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.Skill.InsertSkill;

public record InsertSkillCommand(string Description) : IRequest<ResultViewModel<int>>
{
    public Domain.Entities.Skill ToEntity()
        => new (Description);
    
};