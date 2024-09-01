using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Queries.Skill.GetAllSkills;

public record GetAllSkillsQuery(string Search, int Page, int Size) : IRequest<ResultViewModel<List<SkillItemViewModel>>>;