using DevFreela.Application.DTOs;
using MediatR;

namespace DevFreela.Application.Commands.User.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<ResultViewModel<LoginUserViewModel>>;
