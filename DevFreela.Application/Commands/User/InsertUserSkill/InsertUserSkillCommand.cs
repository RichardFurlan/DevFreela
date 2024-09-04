using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.User.InsertUserSkill;

public record InsertUserSkillCommand(int IdUser, int[] SkillIds) : IRequest<ResultViewModel>;