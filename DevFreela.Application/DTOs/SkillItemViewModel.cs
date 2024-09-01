using DevFreela.Domain.Entities;

namespace DevFreela.Application.DTOs;

public record SkillItemViewModel(int Id, string Description)
{
    public static SkillItemViewModel FromEntity(Skill entity)
        => new(entity.Id, entity.Description);
};