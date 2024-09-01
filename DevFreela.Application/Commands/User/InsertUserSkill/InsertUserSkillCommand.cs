using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUserSkill;

public record InsertUserSkillCommand(int[] SkillIds, int IdUser) : IRequest<ResultViewModel>;