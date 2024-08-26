using DevFreela.Domain.Entities;

namespace DevFreela.API.DTOs;

public record UserViewModel(string FullName, string Email, DateTime BirthDate, List<string> Skills)
{
    public static UserViewModel FromEntity(User entity)
        => new(entity.FullName, entity.Email, entity.BirthDate, entity.Skills.Select(u => u.Skill.Description).ToList());
};